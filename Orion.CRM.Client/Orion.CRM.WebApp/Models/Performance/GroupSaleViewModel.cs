using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Performance
{
    public class GroupSaleViewModel
    {
        // 用户所属项目
        public int? UserProjectId { get; set; }
        /// <summary>
        /// 组成员分析的 开始时间
        /// </summary>
        public string StartDateMember { get; set; }
        /// <summary>
        /// 组成员分析的 截止时间
        /// </summary>
        public string EndDateMember { get; set; }
        /// <summary>
        /// 组变化趋势的 开始时间
        /// </summary>
        public string StartDateTrend { get; set; }
        /// <summary>
        /// 组变化趋势的 截止时间
        /// </summary>
        public string EndDateTrend { get; set; }
        public List<Models.Project.Project> ProjectList { get; set; }
        /// <summary>
        /// 业务组
        /// </summary>
        public List<Models.Group.Group> GroupList { get; set; }
    }
}
