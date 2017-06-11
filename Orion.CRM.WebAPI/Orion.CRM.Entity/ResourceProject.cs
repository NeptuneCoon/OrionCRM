using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    /// <summary>
    /// ResourceProject实体类
    /// </summary>
    public class ResourceProject
    {
        public int Id { get; set; }
        public int ResourceId { get; set; }
        public int ProjectId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
