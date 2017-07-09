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
        public int? SourceFrom { get; set; }
        public int? Status { get; set; }
        public int? Inclination { get; set; }
        public int? TalkCount { get; set; }
        public string Message { get; set; }
        public string Mobile { get; set; }
        public string Tel { get; set; }
        public string QQ { get; set; }
        public string Wechat { get; set; }
        public string Email { get; set; }
        public string Remark { get; set; }
        public string InvalidReason { get; set; }
        public int AppendUserId { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public int ProjectId { get; set; }
        public int GroupId { get; set; }
        public int UserId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 业务员
        /// </summary>
        public string SaleMan { get; set; }
        /// <summary>
        /// 添加人
        /// </summary>
        public string AppendMan { get; set; }
        
    }
}

