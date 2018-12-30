namespace LogicBuilder.Forms.Parameters
{
    public class ConnectorParameters
    {
        public ConnectorParameters(int Id, string ShortString, string LongString, object ConnectorData)
        {
            this.Id = Id;
            this.ShortString = ShortString;
            this.LongString = LongString;
            this.ConnectorData = ConnectorData;
        }

        public ConnectorParameters()
        {
        }

        public int Id { get; set; }
        public string ShortString { get; set; }
        public string LongString { get; set; }
        public object ConnectorData { get; set; }
    }
}
