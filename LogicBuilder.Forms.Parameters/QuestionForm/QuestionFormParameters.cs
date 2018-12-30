using System;
using System.Collections.Generic;
using System.Text;

namespace LogicBuilder.Forms.Parameters
{
    public class QuestionFormParameters
    {
        public QuestionFormParameters(object FormData, ICollection<QuestionRowParameters> Rows)
        {
            this.FormData = FormData;
            this.Rows = Rows;
        }

        public QuestionFormParameters()
        {
        }

        public object FormData { get; set; }
        public ICollection<QuestionRowParameters> Rows { get; set; }
    }
}
