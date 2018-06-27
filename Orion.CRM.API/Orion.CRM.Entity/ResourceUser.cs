using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    /// <summary>
    /// ResourceUser实体类
    /// </summary>
    public class ResourceUser
    {
        public int Id { get; set; }
        public int ResourceId { get; set; }
        public int UserId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
