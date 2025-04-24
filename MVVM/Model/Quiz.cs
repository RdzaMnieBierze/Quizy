using Quizy.MVVM.Viewmodel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizy.MVVM.Model
{
    class Quiz : ObservableObject
    {
        ObservableCollection<Question>? Questions { get; set; }
        int QuestionsLenght = 0;
        double score = 0;

        void AddQuestion() { }
        void RemoveQuestion() { }
        void EditQuestion() { }
    }
}
