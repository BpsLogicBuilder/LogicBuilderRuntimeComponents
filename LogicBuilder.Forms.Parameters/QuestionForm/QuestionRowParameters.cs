using System;
using System.Collections.Generic;
using System.Text;

namespace LogicBuilder.Forms.Parameters
{
    public class QuestionRowParameters
    {
        public QuestionRowParameters(int Id, object RowData, ICollection<QuestionColumnParameters> Columns)
        {
            this.Id = Id;
            this.RowData = RowData;
            this.Columns = Columns;
        }

        public QuestionRowParameters()
        {
        }

        public int Id { get; set; }
        public object RowData { get; set; }
        public ICollection<QuestionColumnParameters> Columns { get; set; }
    }
}
