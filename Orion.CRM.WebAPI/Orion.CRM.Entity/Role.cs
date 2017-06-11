using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    /// <summary>
    /// Role实体类
    /// </summary>
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public int OrgId { get; set; }
        public string OrgName { get; set; }
    }
}

