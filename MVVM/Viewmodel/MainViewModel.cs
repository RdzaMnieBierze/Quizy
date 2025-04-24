using Quizy.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Quizy.MVVM.Viewmodel
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public RelayCommand exit => new RelayCommand(execute => ExitButton());
        private void ExitButton()
        {
            Application.Current.Shutdown();
        }
    }
}
