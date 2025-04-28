using Quizy.MVVM.Viewmodel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizy.MVVM.Model
{
    public class Quiz: ObservableCollection<Question>
    {
        int QuestionsLenght = 0;
        double score = 0;
        public string Name;

        public void AddQuestion() { }
        void RemoveQuestion() { }
        void EditQuestion() { }
    }
}
