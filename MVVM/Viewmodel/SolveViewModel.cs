using Microsoft.Win32;
using Microsoft.Windows.Themes;
using Quizy.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Quizy.MVVM.Viewmodel
{
    public class SolveViewModel : ViewModelBase
    {
        private Quiz _quiz;

        private int _currentQuestionIndex = 0;
        private Question _currentQuestion;
        public ObservableCollection<int> SelectedAnswers { get; set; } = new ObservableCollection<int>();

        #region Bindings and commands
        public bool IsAnswer1Selected
        {
            get => SelectedAnswers.Contains(0);
            set
            {
                UpdateSelectedAnswers(0, value);
                OnPropertyChanged(nameof(IsAnswer1Selected));
            }
        }

        public bool IsAnswer2Selected
        {
            get => SelectedAnswers.Contains(1);
            set
            {
                UpdateSelectedAnswers(1, value);
                OnPropertyChanged(nameof(IsAnswer2Selected));
            }
        }

        public bool IsAnswer3Selected
        {
            get => SelectedAnswers.Contains(2);
            set
            {
                UpdateSelectedAnswers(2, value);
                OnPropertyChanged(nameof(IsAnswer3Selected));
            }
        }

        public bool IsAnswer4Selected
        {
            get => SelectedAnswers.Contains(3);
            set
            {
                UpdateSelectedAnswers(3, value);
                OnPropertyChanged(nameof(IsAnswer4Selected));
            }
        }

        private void UpdateSelectedAnswers(int index, bool isSelected)
        {
            if (isSelected)
            {
                if (!SelectedAnswers.Contains(index))
                    SelectedAnswers.Add(index);
            }
            else
            {
                if (SelectedAnswers.Contains(index))
                    SelectedAnswers.Remove(index);
            }
            CommandManager.InvalidateRequerySuggested();
        }


        public string Question => _currentQuestion.Content;
        public string Answer1 => _currentQuestion[0].Content;
        public string Answer2 => _currentQuestion[1].Content;
        public string Answer3 => _currentQuestion[2].Content;
        public string Answer4 => _currentQuestion[3].Content;

        public Quiz Questions => _quiz;



        public ICommand NextQuestionCommand { get; }
        public ICommand EndQuizCommand { get; } 

        public ICommand ReadQuizCommand { get; }




        private bool CanExecuteNextQuestionCommand(object obj)
        {
            if (_currentQuestionIndex >= _quiz.Count)
                return false;
            return SelectedAnswers.Any();
        }

        private bool _is_result = false;
        public bool IsResult => _is_result;

        public void SetResult(bool result)
        {
            _is_result = result;
            OnPropertyChanged(nameof(IsResult));
            OnPropertyChanged(nameof(Result));
            OnPropertyChanged(nameof(Questions));
        }

        public string Result => _quiz.score.ToString() + " / " + _quiz.Count.ToString() + " (" + Math.Round(_quiz.score / _quiz.Count * 100, 2).ToString() + "%)";




        #endregion

        #region Timer
        private System.Windows.Threading.DispatcherTimer _quizTimer;
        private TimeSpan _elapsedTime;

        public string Timer => _elapsedTime.ToString(@"mm\:ss");

        private void UpdateElapsedTimeDisplay()
        {
            OnPropertyChanged(nameof(Timer));
        }

        private void QuizTimer_Tick(object sender, EventArgs e)
        {
            _elapsedTime = _elapsedTime.Add(TimeSpan.FromSeconds(1));
            UpdateElapsedTimeDisplay();
        }



        #endregion
        public SolveViewModel(Quiz quiz)
        {
            _quiz = quiz;
            LoadCurrentQuestion();

            NextQuestionCommand = new RelayCommand(ExecuteNextQuestionCommand, CanExecuteNextQuestionCommand);
            EndQuizCommand = new RelayCommand(ExecuteEndQuizCommand);
            ReadQuizCommand = new RelayCommand(ReadQuiz);

            _elapsedTime = TimeSpan.Zero;
            _quizTimer = new System.Windows.Threading.DispatcherTimer();
            _quizTimer.Interval = TimeSpan.FromSeconds(1);
            _quizTimer.Tick += QuizTimer_Tick;
            _quizTimer.Start();
        }
        public SolveViewModel()
        {
            _quiz = new Quiz();
           
            NextQuestionCommand = new RelayCommand(ExecuteNextQuestionCommand, CanExecuteNextQuestionCommand);
            EndQuizCommand = new RelayCommand(ExecuteEndQuizCommand);
            ReadQuizCommand = new RelayCommand(ReadQuiz);

            _quiz.Add(new Question(new string[] { "Paris", "Berlin", "Madrid", "Warsaw" }, new bool[] { false, false, false, true }, "What is the capital of Poland"));
            _quiz.Add(new Question(new string[] { "Orange", "Yellow", "Blue", "White" }, new bool[] { true, true, false, false }, "What color is orange"));

            LoadCurrentQuestion();

            _elapsedTime = TimeSpan.Zero;

            _quizTimer = new System.Windows.Threading.DispatcherTimer();
            _quizTimer.Interval = TimeSpan.FromSeconds(1);
            _quizTimer.Tick += QuizTimer_Tick;
            _quizTimer.Start();
        }


        private void LoadCurrentQuestion()
        {
            _currentQuestion = _quiz[_currentQuestionIndex];
            SelectedAnswers.Clear();


            OnPropertyChanged(nameof(Question));
            OnPropertyChanged(nameof(Answer1));
            OnPropertyChanged(nameof(Answer2));
            OnPropertyChanged(nameof(Answer3));
            OnPropertyChanged(nameof(Answer4));
            OnPropertyChanged(nameof(IsAnswer1Selected));
            OnPropertyChanged(nameof(IsAnswer2Selected));
            OnPropertyChanged(nameof(IsAnswer3Selected));
            OnPropertyChanged(nameof(IsAnswer4Selected));

        }

        private void ExecuteEndQuizCommand(object obj)
        { 
            _quizTimer.Stop();
            SetResult(true);
        }

        private void ExecuteNextQuestionCommand(object obj)
        {

            double _tmp_score = 0;
            int _nbAnswers = 0;

            for (int i = 0; i < _currentQuestion.Count; i++)
            {
                if (_currentQuestion[i].isCorrect)
                {
                    _nbAnswers++;
                }

            }
            for (int i =0; i< _currentQuestion.Count; i++)
            {
                if (SelectedAnswers.Contains(i))
                {
                    if (_currentQuestion[i].isCorrect)
                    {
                        _tmp_score++;
                    }
                }
              
            }
            _quiz.score += _tmp_score / _nbAnswers;

            _currentQuestionIndex++;
     

            if (_currentQuestionIndex < _quiz.Count)
            {
                LoadCurrentQuestion();
            }
            else
            {
                _quizTimer.Stop();
                SetResult(true);

            }
        }

        public void ReadQuiz(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Pliki JSON (*.JSON)|*.JSON|Wszystkie pliki (*.*)|*.*",
                Title = "Wczytywanie quizu"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string json= File.ReadAllText(openFileDialog.FileName);
                this._quiz = JsonSerializer.Deserialize<Quiz>(json);
                MessageBox.Show("Plik wczytany pomyślnie!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);


            }
        }

    }




    }

