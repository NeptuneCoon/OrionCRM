using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Performance
{
    /// <summary>
    /// 洽谈记录统计分析视图模型
    /// </summary>
    public class TalkRecordStatViewModel
    {
        // 用户所属项目
        public int? UserProjectId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<Models.Project.Project> ProjectList { get; set; }
        /// <summary>
        /// 业务组
        /// </summary>
        public List<Models.Group.Group> GroupList { get; set; }
        /// <summary>
        /// 各业务员电话量统计排名
        /// </summary>
        public List<Models.Performance.TalkcountRank> TalkcountRanks { get; set; }
    }
}
