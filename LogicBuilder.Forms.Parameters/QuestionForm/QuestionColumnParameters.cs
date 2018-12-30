using System;
using System.Collections.Generic;
using System.Text;

namespace LogicBuilder.Forms.Parameters
{
    public class QuestionColumnParameters
    {
        public QuestionColumnParameters(int Id, object ColumnData, ICollection<QuestionParameters> Questions, ICollection<QuestionRowParameters> Rows)
        {
            this.Id = Id;
            this.ColumnData = ColumnData;
            this.Questions = Questions;
            this.Rows = Rows;
        }

        public QuestionColumnParameters()
        {
        }

        public int Id { get; set; }
        public object ColumnData { get; set; }
        public ICollection<QuestionParameters> Questions { get; set; }
        public ICollection<QuestionRowParameters> Rows { get; set; }
    }
}
