using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Resource
{
    public class Resource
    {
        #region 基本字段
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
        public int DeleteFlag { get; set; }
        public int ProjectId { get; set; }
        public int GroupId { get; set; }
        public int UserId { get; set; }
        #endregion

        #region 扩展字段，计算或转化得出
        /// <summary>
        /// 联系方式(由Mobile/Tel/QQ/Wechat等字段计算得出)
        /// </summary>
        public string ContactInfo { get; set; }
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
        /// <summary>
        /// 来源展示文本
        /// </summary>
        public string SourceFromText { get; set; }
        /// <summary>
        /// 意向展示文本
        /// </summary>
        public string InclinationText { get; set; }
        /// <summary>
        /// 状态展示文本
        /// </summary>
        public string StatusText { get; set; } 
        #endregion
    }
}
