using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.CRMLog
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
