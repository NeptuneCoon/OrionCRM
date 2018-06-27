using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    /// <summary>
    /// Organization实体类
    /// </summary>
    public class Organization
    {
        public int Id { get; set; }
        public string OrgName { get; set; }
        public string OrgCode { get; set; }
        public int Type { get; set; }
        public DateTime CreateTime { get; set; }
        public bool DeleteFlag { get; set; }
    }
}

