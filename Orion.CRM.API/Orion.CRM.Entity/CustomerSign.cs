using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    /// <summary>
    /// Resource实体类
    /// </summary>
    public class CustomerSign
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public int? ResourceId { get; set; }
        public DateTime SignTime { get; set; }
        public int SignUserId { get; set; }
        public string SignMan { get; set; }
        public int? GroupId { get; set; }
        public string GroupName { get; set; }
        public int ProjectId { get; set; }
        public int OrgId { get; set; }
        public DateTime CreateTime { get; set; }
        public int AppendUserId { get; set; }
        public string AppendMan { get; set; }
    }
}
