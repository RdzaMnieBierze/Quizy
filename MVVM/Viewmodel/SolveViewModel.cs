using Quizy.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quizy.MVVM.Viewmodel
{
    public class SolveViewModel : ViewModelBase
    {
        private int _currentQuestionIndex = 0;
        private Question _currentQuestion;
        public SolveViewModel()
        {
            LoadCurrentQuestion();
        }

        public string Question => _currentQuestion.Content;
        public string Answer1 => _currentQuestion[0].Content;
        public string Answer2 => _currentQuestion[1].Content;
        public string Answer3 => _currentQuestion[2].Content;
        public string Answer4 => _currentQuestion[3].Content;


        private string _answer="siema";
        public string Answer
        {
            get => _answer;

            set
            {   
                _answer = value;
                OnPropertyChanged(nameof(Answer));
            }
        }

        private void LoadCurrentQuestion()
        {
            _currentQuestion= new Question(new string[] { "Paris", "Berlin", "Madrid", "Warsaw" }, new bool[] { true, false, false, false }, "What is the capital of Poland");

            OnPropertyChanged(nameof(Question));
            OnPropertyChanged(nameof(Answer1));
            OnPropertyChanged(nameof(Answer2));
            OnPropertyChanged(nameof(Answer3));
            OnPropertyChanged(nameof(Answer4));
     
        }




    }
}
