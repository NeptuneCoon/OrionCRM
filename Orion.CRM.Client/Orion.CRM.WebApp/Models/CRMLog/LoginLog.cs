using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.CRMLog
{
    public class LoginLog
    {
        public long Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
        public string IP { get; set; }
        public DateTime LoginTime { get; set; }
    }
}
