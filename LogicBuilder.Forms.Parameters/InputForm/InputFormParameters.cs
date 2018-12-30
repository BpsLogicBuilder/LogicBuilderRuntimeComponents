using System.Collections.Generic;

namespace LogicBuilder.Forms.Parameters
{
    public class InputFormParameters
    {
        public InputFormParameters(object FormData, ICollection<InputRowParameters> Rows)
        {
            this.FormData = FormData;
            this.Rows = Rows;
        }

        public InputFormParameters()
        {
        }

        public object FormData { get; set; }
        public ICollection<InputRowParameters> Rows { get; set; }
    }
}
