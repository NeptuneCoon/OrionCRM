using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.AppUser
{
    public class AppUserSearchParams
    {
        /// <summary>
        /// 查询关键字
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// 组织机构Id
        /// </summary>
        public int oid { get; set; }
        /// <summary>
        /// 业务组Id
        /// </summary>
        public int? gid { get; set; }
        /// <summary>
        /// 角色id
        /// </summary>
        public int? roleid { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int pi { get; set; }
        /// <summary>
        /// 页容量
        /// </summary>
        public int ps { get; set; }
        /// <summary>
        /// 谈单人(0=不是，1=是)
        /// </summary>
        public int? tk { get; set; }
    }
}
