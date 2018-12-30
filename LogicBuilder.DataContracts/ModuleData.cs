using System;
using System.Collections.Generic;
using System.Text;

namespace LogicBuilder.DataContracts
{
    public class ModuleData
    {
        public byte[] RulesStream { get; set; }
        public byte[] ResourcesStream { get; set; }
        public string ModuleName { get; set; }
        public string Application { get; set; }
        public string UserData { get; set; }
        public string UploadedBy { get; set; }
        public DateTime UploadedTime { get; set; }
    }
}
