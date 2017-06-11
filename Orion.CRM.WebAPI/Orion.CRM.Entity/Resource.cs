using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    /// <summary>
    /// Resource实体类
    /// </summary>
    public class Resource
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public int? Sex { get; set; }
        public string Address { get; set; }
        public DateTime? MsgTime { get; set; }
        public DateTime? LastTime { get; set; }
        public string SourceFrom { get; set; }
        public int? Status { get; set; }
        public string IntentionRank { get; set; }
        public int? TalkCount { get; set; }
        public string Message { get; set; }
        public string Mobile { get; set; }
        public string Tel { get; set; }
        public string QQ { get; set; }
        public string Wechat { get; set; }
        public string Email { get; set; }
        public DateTime? ImportTime { get; set; }
        public string InvalidReason { get; set; }
        public string AppendMan { get; set; }
        public int DeleteFlag { get; set; }
    }
}

