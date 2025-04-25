using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizy.MVVM.Model
{
    public class Answer
    {
        private string _content;
        public string Content { get { return _content; } set { _content = value; } }
        bool isCorrect;

        public Answer(string content, bool isCorrect)
        {
            Content = content;
            this.isCorrect = isCorrect;
        }
    }
}
