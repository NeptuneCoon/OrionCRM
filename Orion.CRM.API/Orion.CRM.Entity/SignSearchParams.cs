using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    public class SignSearchParams
    {
        /// <summary>
        /// 客户姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 联系方式(手机，固话，微信，QQ等)
        /// </summary>
        public string con { get; set; }
        /// <summary>
        /// 签约用户的UserId
        /// </summary>
        public int uid { get; set; }
        /// <summary>
        /// 组织机构Id
        /// </summary>
        public int oid { get; set; }
        /// <summary>
        /// 项目Id
        /// </summary>
        public int pid { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int pi { get; set; }
        /// <summary>
        /// 页容量
        /// </summary>
        public int ps { get; set; }
    }
}
