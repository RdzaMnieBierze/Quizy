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
        private static Quiz? CurrentQuiz;
        public RelayCommand Add => new RelayCommand(execute => AddQuestion());
        public RelayCommand Previous => new RelayCommand(execute => PreviousQuestion());
        public RelayCommand Next => new RelayCommand(execute => NextQuestion());
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
        public CreateViewModel()
        {
            if (CurrentQuiz == null)
                CurrentQuiz = new Quiz();
        }
        private void AddQuestion()
        {
            if (CurrentQuiz == null)
                return;
            if (QuestionId < CurrentQuiz.Count)
            {
                MessageBox.Show("To pytanie już istnieje. Przejdź na koniec quizu, aby dodać nowe pytanie.", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            CurrentQuiz?.Add(new Model.Question(Answers, Checked, Question));
            MessageBox.Show(Question + Answers[0] + Checked[0] + Answers[1] + Checked[1]);
            Clear();

            QuestionId = CurrentQuiz.Count;
            OnPropertyChanged(nameof(QuestionId));
            OnPropertyChanged(nameof(QuizLenght));
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
            OnPropertyChanged(nameof(QuizLenght));
        }
        private void PreviousQuestion()
        {
            if (CurrentQuiz == null || CurrentQuiz.Count == 0)
                return;

            if (QuestionId > 0)
            {
                QuestionId--;
                LoadQuestion(QuestionId);
            }
        }
        private void NextQuestion()
        {
            if (CurrentQuiz == null || CurrentQuiz.Count == 0)
                return;

            if (QuestionId < CurrentQuiz.Count - 1)
            {
                QuestionId++;
                LoadQuestion(QuestionId);
            }
        }
    }
}
