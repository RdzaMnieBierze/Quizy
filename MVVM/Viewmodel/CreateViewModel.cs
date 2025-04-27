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
       
        private void AddQuestion()
        {
            CurrentQuiz?.Add(new Model.Question(Answers, Checked, Question));
            MessageBox.Show(Question + Answers[0] + Checked[0] + Answers[1] + Checked[1]);
            Clear();
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
    }
}
