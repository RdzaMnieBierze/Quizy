using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Quizy.MVVM.Viewmodel
{
    static class AesEncryption
    {
        static public byte[] Encrypt(string plainText, byte[] key, byte[] iv)
        {
            byte[] cipheredtext;

            using (Aes aes = Aes.Create())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(key, iv);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                        cipheredtext = ms.ToArray();
                    }
                }
            }
            return cipheredtext;
        }

        static public string Decrypt(byte[] cipheredtext, byte[] key, byte[] iv)
        {
            string simpletext = String.Empty;
            using (Aes aes = Aes.Create())
            {
                ICryptoTransform decryptor = aes.CreateDecryptor(key, iv);
                using (MemoryStream memoryStream = new MemoryStream(cipheredtext))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            try
                            {
                                simpletext = streamReader.ReadToEnd();
                            }
                            catch (CryptographicException e)
                            {

                                MessageBox.Show("Decryption failed: " + e.Message);
                            }
                            catch (Exception e)
                            {

                                MessageBox.Show("An error occurred: " + e.Message);
                            }
                            finally
                            {
                                streamReader.Close();

                            }
                        }
                    }
                }
            }
            return simpletext;
        }
    }
}
