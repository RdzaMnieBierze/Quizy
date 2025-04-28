using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Quizy.MVVM.Model;

namespace Quizy.MVVM.Viewmodel
{
    class CreateViewModel : ViewModelBase
    {
        private Quiz? CurrentQuiz;
        public RelayCommand Add => new RelayCommand(execute => AddQuestion());
        public bool CanAddNewQuestion => QuestionId >= (CurrentQuiz?.Count ?? 0);
        public RelayCommand Previous => new RelayCommand(execute => PreviousQuestion());
        public RelayCommand Next => new RelayCommand(execute => NextQuestion());
        public RelayCommand NewQuestion => new RelayCommand(execute => CreateNewQuestion());
        public RelayCommand Delete => new RelayCommand(execute => DeleteQuestion());
        public bool CanGoPrevious => QuestionId > 0;
        public bool CanGoNext => CurrentQuiz != null && QuestionId < CurrentQuiz.Count - 1;
        public int QuizLenght => CurrentQuiz?.Count ?? 0;
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
            get => (_questionId + 1).ToString() + " / " + (CurrentQuiz.Count).ToString();
            set
            {
                OnPropertyChanged(nameof(QuestionView));
            }
        }
        public CreateViewModel()
        {
            if (CurrentQuiz == null)
                CurrentQuiz = new Quiz();
        }
        private void SaveCurrentQuestion()
        {
            if (CurrentQuiz == null)
                return;

            if (QuestionId < CurrentQuiz.Count)
            {
                for (int i = 0; i < Answers.Length; i++)
                {
                    CurrentQuiz[QuestionId][i].Content = Answers[i];
                    CurrentQuiz[QuestionId][i].isCorrect = Checked[i];
                }
            }
            else
            {
                CurrentQuiz.Add(new Model.Question(Answers, Checked, Question));
            }

        }
        private void CreateNewQuestion()
        {
            Clear();
            QuestionId = CurrentQuiz?.Count ?? 0;
            OnPropertyChanged(nameof(QuestionId));
            OnPropertyChanged(nameof(QuestionView));
        }
        private void AddQuestion()
        {
            if (CurrentQuiz == null)
                return;

            if (QuestionId < CurrentQuiz.Count)
            {
                SaveCurrentQuestion();
                MessageBox.Show("Zapisano zmiany w istniejącym pytaniu.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                CurrentQuiz.Add(new Model.Question(Answers, Checked, Question));
                MessageBox.Show("Dodano nowe pytanie.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            Clear();
            QuestionId = CurrentQuiz.Count;
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
            if (CurrentQuiz == null || id < 0 || id >= CurrentQuiz.Count)
                return;


            for (int i = 0; i < CurrentQuiz?[QuestionId].Count; i++)
            {
                Answers[i] = CurrentQuiz[QuestionId][i].Content;
                Checked[i] = CurrentQuiz[QuestionId][i].isCorrect;
            }
            Question = CurrentQuiz[QuestionId].Content;
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
            if (CurrentQuiz == null || CurrentQuiz.Count == 0)
                return;

            if (QuestionId >= 0 && QuestionId < CurrentQuiz.Count)
            {
                CurrentQuiz.RemoveAt(QuestionId);
                MessageBox.Show("Pytanie zostało usunięte.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);

                if (QuestionId >= CurrentQuiz.Count)
                {
                    QuestionId = CurrentQuiz.Count - 1;
                }

                if (CurrentQuiz.Count > 0 && QuestionId >= 0)
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
