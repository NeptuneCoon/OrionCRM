using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    /// <summary>
    /// ResourceOrganization实体类
    /// </summary>
    public class ResourceOrganization
    {
        public int Id { get; set; }
        public int ResourceId { get; set; }
        public int OrgId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
