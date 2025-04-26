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
        private Quiz CurrentQuiz;
        public RelayCommand Add => new RelayCommand(_ => AddQuestion());


        private string _question;
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
       
        private void AddQuestion()
        {
            //CurrentQuiz.Add(new Model.Question(Answers,))
            //MessageBox.Show(Question+ Answers[0]+ Answers[1]);
        }
    }
}
