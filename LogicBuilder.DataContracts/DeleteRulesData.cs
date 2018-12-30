using System;

namespace LogicBuilder.DataContracts
{
    public class DeleteRulesData
    {
        public string[] Files { get; set; }
        public string Application { get; set; }
        public string UserData { get; set; }
        public string DeletedBy { get; set; }
        public DateTime DeleteTime { get; set; }
    }
}
