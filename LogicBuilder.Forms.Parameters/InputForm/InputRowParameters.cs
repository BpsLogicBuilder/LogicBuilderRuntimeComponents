using System.Collections.Generic;

namespace LogicBuilder.Forms.Parameters
{
    public class InputRowParameters
    {
        public InputRowParameters(int Id, object RowData, ICollection<InputColumnParameters> Columns)
        {
            this.Id = Id;
            this.RowData = RowData;
            this.Columns = Columns;
        }

        public InputRowParameters()
        {
        }

        public int Id { get; set; }
        public object RowData { get; set; }
        public ICollection<InputColumnParameters> Columns { get; set; }
    }
}
