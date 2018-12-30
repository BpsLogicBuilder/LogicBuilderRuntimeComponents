using System;

namespace LogicBuilder.DataContracts
{
    public class VariableMetaData
    {
        public string XmlData { get; set; }
        public string Application { get; set; }
        public string UserData { get; set; }
        public string UploadedBy { get; set; }
        public DateTime UploadedTime { get; set; }
    }
}
