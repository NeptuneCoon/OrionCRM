using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Sign
{
    /// <summary>
    /// 签约记录视图模型
    /// </summary>
    public class SignViewModel
    {
        /// <summary>
        /// 组名
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 组业绩(销售额)
        /// </summary>
        public int GroupSaleAmount { get; set; }
    }
}
