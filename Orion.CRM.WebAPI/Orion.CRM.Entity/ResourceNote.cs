using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    /// <summary>
    /// ResourceNote实体类
    /// </summary>
    public class ResourceNote
    {
        public int Id { get; set; }
        public int ResourceId { get; set; }
        public string Message { get; set; }
        public bool IsRemind { get; set; }
        public DateTime? RemindTime { get; set; }
        public int UserId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
