using Quizy.MVVM.Viewmodel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;

namespace Quizy.MVVM.Model
{
    public class Question
    {
        public string Content { get; set; }
        public List<Answer> Answers { get; set; } = new();

        public Question() { }

        public Question(string[] ans, bool[] corr, string content)
        {
            Content = content;
            for (int i = 0; i < ans.Length; i++)
            {
                Answers.Add(new Answer(ans[i], corr[i]));
            }
        }

        public override string ToString()
        {
            string tmp = Content + " | poprawne odpowiedzi: ";
            foreach (var ans in Answers)
            {
                if (ans.isCorrect)
                {
                    tmp += ans.Content + " ";
                }
            }
            return tmp;
        }
    }
}

