using Quizy.MVVM.Viewmodel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizy.MVVM.Model
{
    public class Question : ObservableCollection<Answer>
    {


        private string _content;
        public string Content { get { return _content; } }
        public Question(string[] ans, bool[] corr, string _content)
        {
            this._content = _content;

            for (int i = 0; i < ans.Length; i++)
            {
                Add(new Answer(ans[i], corr[i]));
            }
        }


        public override string ToString()

        {
            string tmp = Content+ "| poprawne odpowiedzi: ";
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].isCorrect)
                {
                    tmp+= this[i].Content + " ";
                }
            }
            return tmp;
        }
    }
}
