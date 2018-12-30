using System;
using System.Collections.Generic;
using System.Text;

namespace LogicBuilder.Forms.Parameters
{
    public class AnswerParameters
    {
        public AnswerParameters(string Text, object AnswerData)
        {
            this.Text = Text;
            this.AnswerData = AnswerData;
        }

        public AnswerParameters()
        {
        }

        public string Text { get; set; }
        public object AnswerData { get; set; }
    }
}
