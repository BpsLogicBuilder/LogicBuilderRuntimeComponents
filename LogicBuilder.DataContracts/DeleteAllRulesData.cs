using System;
using System.Collections.Generic;
using System.Text;

namespace LogicBuilder.DataContracts
{
    public class DeleteAllRulesData
    {
        public string Application { get; set; }
        public string UserData { get; set; }
        public string DeletedBy { get; set; }
        public DateTime DeleteTime { get; set; }
    }
}
