using System;
using System.Collections.Generic;
using System.Text;

namespace LogicBuilder.Forms.Parameters
{
    public class QuestionParameters
    {
        public QuestionParameters(int Id, string Text, object QuestionData, ICollection<AnswerParameters> Answers)
        {
            this.Id = Id;
            this.Text = Text;
            this.QuestionData = QuestionData;
            this.Answers = Answers;
        }

        public QuestionParameters()
        {
        }

        public int Id { get; set; }
        public string Text { get; set; }
        public object QuestionData { get; set; }
        public ICollection<AnswerParameters> Answers { get; set; }
    }
}
