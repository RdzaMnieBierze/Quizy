using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Quizy.MVVM.Model;
using System.Text.Json;
using System.IO;
using System.Security.Cryptography;

namespace Quizy.MVVM.Viewmodel
{
    class CreateViewModel : ViewModelBase
    {
        private Quiz? CurrentQuiz;
        public RelayCommand Add => new RelayCommand(execute => AddQuestion());
        public bool CanAddNewQuestion => QuestionId >= (CurrentQuiz?.Questions.Count ?? 0);
        public RelayCommand Previous => new RelayCommand(execute => PreviousQuestion());
        public RelayCommand Next => new RelayCommand(execute => NextQuestion());
        public RelayCommand NewQuestion => new RelayCommand(execute => CreateNewQuestion());
        public RelayCommand Delete => new RelayCommand(execute => DeleteQuestion());
        public bool CanGoPrevious => QuestionId > 0;
        public bool CanGoNext => CurrentQuiz != null && QuestionId < CurrentQuiz.Questions.Count - 1;
        public ICommand SaveQuizModeCommand { get; }
        public ICommand SaveQuizCommand { get; }
        public ICommand CancelSaveCommand { get; }
        public int QuizLenght => CurrentQuiz?.Questions.Count ?? 0;
        private int _questionId = 0;
        public int QuestionId
        {
            get => _questionId;
            set
            {
                _questionId = value;
                OnPropertyChanged(nameof(QuestionId));
            }
        }
        private string? _question;
        public string Question
        {
            get => _question; 
            set
            {
                _question = value;
                OnPropertyChanged(nameof(Question));
            }
        }
        private string[] _answers = new string[4];
        public string[] Answers
        {
            get => _answers;
            set
            {
                _answers = value;
                OnPropertyChanged(nameof(Answers));
            }
        }
        private bool[] _checked = new bool[4];
        public bool[] Checked
        {
            get => _checked;
            set
            {
                _checked = value;
                OnPropertyChanged(nameof(Checked));
            }
        }
        public string QuestionView
        {
            get => (_questionId + 1).ToString() + " / " + (CurrentQuiz.Questions.Count).ToString();
            set
            {
                OnPropertyChanged(nameof(QuestionView));
            }
        }
        private bool _isSavingMode;
        public bool IsSavingMode
        {
            get => _isSavingMode;
            set
            {
                _isSavingMode = value;
                OnPropertyChanged(nameof(IsSavingMode));
            }
        }

        private string _quizName;
        public string QuizName
        {
            get => _quizName;
            set
            {
                _quizName = value;
                OnPropertyChanged(nameof(QuizName));
            }
        }
        public CreateViewModel()
        {
            if (CurrentQuiz == null)
                CurrentQuiz = new Quiz();
            SaveQuizModeCommand = new RelayCommand(_ => IsSavingMode = true);
            SaveQuizCommand = new RelayCommand(_ => SaveQuiz());
            CancelSaveCommand = new RelayCommand(_ => IsSavingMode = false);
        }
        private void SaveQuiz()
        {
            if (CurrentQuiz == null)
                return;

            CurrentQuiz.Name = QuizName;

            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = QuizName,
                DefaultExt = ".json",
                Filter = "Pliki JSON (*.json)|*.json|Wszystkie pliki (*.*)|*.*"
            };

            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                string filePath = saveFileDialog.FileName;

                string json = JsonSerializer.Serialize(CurrentQuiz, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                try
                {
                    byte[] key = new byte[16] {
                        0x1F, 0xA2, 0x3C, 0x4B,
                        0x5E, 0x67, 0x88, 0x9D,
                        0xAB, 0xBC, 0xCD, 0xDE,
                        0xEF, 0xF0, 0x12, 0x34
                    };


                    byte[] iv = new byte[16] {
                        0x00, 0x11, 0x22, 0x33,
                        0x44, 0x55, 0x66, 0x77,
                        0x88, 0x99, 0xAA, 0xBB,
                        0xCC, 0xDD, 0xEE, 0xFF};

             

                    byte[] encryptedData = AesEncryption.Encrypt(json, key, iv);



                    File.WriteAllBytes(filePath, encryptedData);
                    MessageBox.Show("Quiz zapisano pomyślnie!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                    IsSavingMode = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Wystąpił błąd podczas zapisu pliku:\n{ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool CheckQuestion()
        {
            if (Question == string.Empty)
            {
                return false;
            }
            for (int i = 0; i < Answers.Length; i++)
            {
                if (Answers[i] == string.Empty) return false;
            }
            for (int i = 0; i < Checked.Length; i++)
            {
                if (Checked[i] == true) return true;
            }
            return false;
        }
        private void SaveCurrentQuestion()
        {
            if (CurrentQuiz == null)
                return;

            if (QuestionId < CurrentQuiz.Questions.Count)
            {
                for (int i = 0; i < Answers.Length; i++)
                {
                    CurrentQuiz.Questions[QuestionId].Answers[i].Content = Answers[i];
                    CurrentQuiz.Questions[QuestionId].Answers[i].isCorrect = Checked[i];
                }
                CurrentQuiz.Questions[QuestionId].Content = Question;
            }
            else
            {
                CurrentQuiz.Questions.Add(new Model.Question(Answers, Checked, Question));
            }

        }
        private void CreateNewQuestion()
        {
            Clear();
            QuestionId = CurrentQuiz?.Questions.Count ?? 0;
            OnPropertyChanged(nameof(QuestionId));
            OnPropertyChanged(nameof(QuestionView));
        }
        private void AddQuestion()
        {
            if (CurrentQuiz == null)
                return;

            if (!CheckQuestion())
            {
                MessageBox.Show("Nie można zapisać nie pełnego pytania.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (QuestionId < CurrentQuiz.Questions.Count)
            {
                SaveCurrentQuestion();
                MessageBox.Show("Zapisano zmiany w istniejącym pytaniu.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                CurrentQuiz.Questions.Add(new Model.Question(Answers, Checked, Question));
                MessageBox.Show("Dodano nowe pytanie.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            Clear();
            QuestionId = CurrentQuiz.Questions.Count;
            OnPropertyChanged(nameof(QuizLenght));
            OnPropertyChanged(nameof(QuestionId));
            OnPropertyChanged(nameof(QuestionView));
            OnPropertyChanged(nameof(CanAddNewQuestion));
            OnPropertyChanged(nameof(CanGoPrevious));
            OnPropertyChanged(nameof(CanGoNext));
        }
        private void Clear()
        {
            for(int i = 0; i < Answers.Length; i++)
            {
                Answers[i] = string.Empty;
            }
            for(int i = 0; i < Checked.Length; i++)
            {
                Checked[i] = false;
            }
            Question = string.Empty;
            OnPropertyChanged(nameof (Question));
            OnPropertyChanged(nameof (Answers));
            OnPropertyChanged(nameof (Checked));
        }


        private void LoadQuestion(int id)
        {
            if (CurrentQuiz == null || id < 0 || id >= CurrentQuiz.Questions.Count)
                return;


            for (int i = 0; i < CurrentQuiz?.Questions[QuestionId].Answers.Count; i++)
            {
                Answers[i] = CurrentQuiz.Questions[QuestionId].Answers[i].Content;
                Checked[i] = CurrentQuiz.Questions[QuestionId].Answers[i].isCorrect;
            }
            Question = CurrentQuiz.Questions[QuestionId].Content;
            OnPropertyChanged(nameof(Question));
            OnPropertyChanged(nameof(Answers));
            OnPropertyChanged(nameof(Checked));
            OnPropertyChanged(nameof(QuestionId));
            OnPropertyChanged(nameof(QuestionView));
            OnPropertyChanged(nameof(QuizLenght));
            OnPropertyChanged(nameof(CanGoPrevious));
            OnPropertyChanged(nameof(CanGoNext));
        }
        private void PreviousQuestion()
        {
            if (CanGoPrevious)
            {
                QuestionId--;
                LoadQuestion(QuestionId);
            }
            OnPropertyChanged(nameof(CanGoPrevious));
            OnPropertyChanged(nameof(CanGoNext));
        }
        private void NextQuestion()
        {
            if (CanGoNext)
            {
                QuestionId++;
                LoadQuestion(QuestionId);
            }
            OnPropertyChanged(nameof(CanGoPrevious));
            OnPropertyChanged(nameof(CanGoNext));
        }
        private void DeleteQuestion()
        {
            if (CurrentQuiz == null || CurrentQuiz.Questions.Count == 0)
                return;

            if (QuestionId >= 0 && QuestionId < CurrentQuiz.Questions.Count)
            {
                CurrentQuiz.Questions.RemoveAt(QuestionId);
                MessageBox.Show("Pytanie zostało usunięte.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);

                if (QuestionId >= CurrentQuiz.Questions.Count)
                {
                    QuestionId = CurrentQuiz.Questions.Count - 1;
                }

                if (CurrentQuiz.Questions.Count > 0 && QuestionId >= 0)
                {
                    LoadQuestion(QuestionId);
                }
                else
                {
                    Clear();
                    QuestionId = 0;
                }

                OnPropertyChanged(nameof(QuizLenght));
                OnPropertyChanged(nameof(CanAddNewQuestion));
                OnPropertyChanged(nameof(CanGoPrevious));
                OnPropertyChanged(nameof(CanGoNext));
                OnPropertyChanged(nameof(QuestionId));
                OnPropertyChanged(nameof(QuestionView));
            }
        }
    }
}
