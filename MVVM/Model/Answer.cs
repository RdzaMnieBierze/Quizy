using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizy.MVVM.Model
{
    public class Answer
    {
        string Content {  get; set; }
        bool isCorrect;

        public Answer(string content, bool isCorrect)
        {
            Content = content;
            this.isCorrect = isCorrect;
        }
    }
}
