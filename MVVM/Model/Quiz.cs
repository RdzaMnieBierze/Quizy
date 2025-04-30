using Quizy.MVVM.Viewmodel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizy.MVVM.Model
{
    public class Quiz
    {
        public double score { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Question> Questions { get; set; } = new();

        public Quiz() { }
    }
}
