using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Performance
{
    public class TalkcountRank
    {
        /// <summary>
        /// 业务员姓名
        /// </summary>
        public string Saleman { get; set; }

        /// <summary>
        /// 洽谈次数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 占比
        /// </summary>
        public string Percent { get; set; }

    }
}
