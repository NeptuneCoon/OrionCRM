using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity.CRMLog
{
    public class ErrorLog
    {
        public long Id { get; set; }
        public int Origin { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public string Parameters { get; set; }
        public string ErrorMsg { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
