using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Performance
{
    /// <summary>
    /// 某段时间内组业绩数据
    /// </summary>
    public class GroupSaleRanking
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string ProjectName { get; set; }
        /// <summary>
        /// 所选时间段内的业绩
        /// </summary>
        public int TotalAmount { get; set; }
    }
}
