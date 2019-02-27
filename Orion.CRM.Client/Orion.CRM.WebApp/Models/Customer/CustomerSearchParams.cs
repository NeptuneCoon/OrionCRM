using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Customer
{
    public class CustomerSearchParams
    {
        public string name { get; set; }
        public string mobile { get; set; }
        /// <summary>
        /// 项目
        /// </summary>
        public int? pid { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public int? bid { get; set; }
        /// <summary>
        /// 代理级别
        /// </summary>
        public int? level { get; set; }
        /// <summary>
        /// 代理区域1
        /// </summary>
        public string z1 { get; set; }
        /// <summary>
        /// 代理区域2
        /// </summary>
        public string z2 { get; set; }
        /// <summary>
        /// 代理区域3
        /// </summary>
        public string z3 { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int pi { get; set; }
        /// <summary>
        /// 页容量
        /// </summary>
        public int ps { get; set; }
        /// <summary>
        /// 售后服务专员Id
        /// </summary>
        public int? suid { get; set; }
    }
}
