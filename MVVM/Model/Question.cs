using Quizy.MVVM.Viewmodel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizy.MVVM.Model
{
    class Question
    {
        ObservableCollection<Answer>? Answers;
        string Content;
        Question(string[] ans, bool[] corr, string _content)
        {
            Content = _content;

            for(int i = 0; i < ans.Length; i++)
            {
                Answers.Add(new Answer(ans[i], corr[i]));
            }
        }
    }
}
