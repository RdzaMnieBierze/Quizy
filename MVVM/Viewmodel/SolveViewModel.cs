using Quizy.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Quizy.MVVM.Viewmodel
{
    public class SolveViewModel : ViewModelBase
    {
        private Quiz _quiz;

        private int _currentQuestionIndex = 0;
        private int _selectedAnswerIndex = -1;
        private Question _currentQuestion;
 

        public string Question => _currentQuestion.Content;
        public string Answer1 => _currentQuestion[0].Content;
        public string Answer2 => _currentQuestion[1].Content;
        public string Answer3 => _currentQuestion[2].Content;
        public string Answer4 => _currentQuestion[3].Content;

        public int SelectedAnswerIndex
        {
            get => _selectedAnswerIndex;
            set
            {
                if (_selectedAnswerIndex != value)
                {
                    _selectedAnswerIndex = value;
                    OnPropertyChanged(nameof(SelectedAnswerIndex));
                }
            }
        }

        public ICommand NextQuestionCommand { get; }


        public SolveViewModel()
        {
            _quiz = new Quiz();
           
            NextQuestionCommand = new RelayCommand(ExecuteNextQuestionCommand);
            _quiz.Add(new Question(new string[] { "Paris", "Berlin", "Madrid", "Warsaw" }, new bool[] { false, false, false, true }, "What is the capital of Poland"));
            _quiz.Add(new Question(new string[] { "Orange", "Yellow", "Blue", "White" }, new bool[] { true, false, false, false }, "What color is orange"));

            LoadCurrentQuestion();
        }


        private void LoadCurrentQuestion()
        {
            _currentQuestion = _quiz[_currentQuestionIndex];
            _selectedAnswerIndex = -1; // Reset selected answer index


            OnPropertyChanged(nameof(Question));
            OnPropertyChanged(nameof(Answer1));
            OnPropertyChanged(nameof(Answer2));
            OnPropertyChanged(nameof(Answer3));
            OnPropertyChanged(nameof(Answer4));
            OnPropertyChanged(nameof(SelectedAnswerIndex));

        }

        private void ExecuteNextQuestionCommand(object obj)
        {
            if (_currentQuestion[_selectedAnswerIndex].isCorrect)
            {
                _quiz.score++;
            }

            _currentQuestionIndex++;
            MessageBox.Show(_quiz.score.ToString());

            if (_currentQuestionIndex < _quiz.Count)
            {
                LoadCurrentQuestion();
            }
            else
            {
                MessageBox.Show("Koniec");
               
            }
        }




    }
}
