using System;
using System.Collections.Generic;
using System.Text;

namespace LogicBuilder.Forms.Parameters
{
    public class InputColumnParameters
    {
        public InputColumnParameters(int Id, object ColumnData, ICollection<BaseInputQuestionParameters> Questions, ICollection<InputRowParameters> Rows)
        {
            this.Id = Id;
            this.ColumnData = ColumnData;
            this.Questions = Questions;
            this.Rows = Rows;
        }

        public InputColumnParameters()
        {
        }

        public int Id { get; set; }
        public object ColumnData { get; set; }
        public ICollection<BaseInputQuestionParameters> Questions { get; set; }
        public ICollection<InputRowParameters> Rows { get; set; }
    }
}
