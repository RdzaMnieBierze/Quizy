using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Quizy.Core;

namespace Quizy.MVVM.Viewmodel
{
    class MainViewModel : ViewModelBase
    {
        public RelayCommand exit => new RelayCommand(execute => ExitButton());
        private void ExitButton()
        {
            Application.Current.Shutdown();
        }

        private readonly NavigationService _navigationService;
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public ICommand NavigateCreate { get; }
        public ICommand NavigateSolve { get; }

        public MainViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;
            _navigationService.SetNavigator(vm => CurrentViewModel = vm);

            ViewModelBase createvm = new CreateViewModel();
            ViewModelBase solvevm = new SolveViewModel();

            NavigateCreate = new RelayCommand(_ => _navigationService.NavigateTo(createvm), _ => true);
            NavigateSolve = new RelayCommand(_ => _navigationService.NavigateTo(solvevm), _ => true);

            CurrentViewModel = createvm;
        }
    }
}
