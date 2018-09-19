using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity.CRMLog
{
    public class ActionLog
    {
        public long Id { get; set; }
        public string PageURL { get; set; }
        public string QueryString { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
