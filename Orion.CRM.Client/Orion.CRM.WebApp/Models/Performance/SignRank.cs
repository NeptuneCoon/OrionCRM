using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Performance
{
    public class SignRank
    {
        /// <summary>
        /// 签约人，指业务人员
        /// </summary>
        public string SignMan { get; set; }
        /// <summary>
        /// 签约总金额
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// 占比
        /// </summary>
        public string Percent { get; set; }
    }
}
