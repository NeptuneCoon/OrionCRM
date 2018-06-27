using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Resource
{
    /// <summary>
    /// Resource实体，和Resource表中的字段一一对应
    /// </summary>
    public class ResourceEntity
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Message { get; set; }
        public int? Sex { get; set; }
        public string Mobile { get; set; }
        public string Tel { get; set; }
        public string Wechat { get; set; }
        public string QQ { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime? MsgTime { get; set; }
        public DateTime? LastTime { get; set; }
        public int? Inclination { get; set; }
        public int TalkCount { get; set; }
        public string InvalidReason { get; set; }
        public int? SourceFrom { get; set; }
        public int Status { get; set; }
        public int? AppendUserId { get; set; }
        public string Remark { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
