using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity.CRMLog
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
