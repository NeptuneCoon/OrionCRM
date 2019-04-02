using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    public class Customer
    {
        public int Id { get; set; }
        public string RealName { get; set; }
        /// <summary>
        /// 客户类型（1=名额预定，2=签约合作）
        /// </summary>
        public int? Type { get; set; }
        public int? Sex { get; set; }
        public string Mobile { get; set; }
        public string IdentityNo { get; set; }
        public int BrandId { get; set; }
        /// <summary>
        /// 代理级别：1=省，2=市，3=县/区，4=直辖市
        /// </summary>
        public int AgentLevel { get; set; }
        public string AgentZone1 { get; set; }
        public string AgentZone2 { get; set; }
        public string AgentZone3 { get; set; }
        public int ProjectId { get; set; }
        public int? ServiceUserId { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

        // 扩展属性
        public string ProjectName { get; set; }
        public string BrandName { get; set; }
        public string ServiceRealName { get; set; }//售后服务专员姓名
    }

    public class CustomerSearchParams
    {
        public string name { get; set; }
        public string mobile { get; set; }
        /// <summary>
        /// 客户类型（1=名额预定，2=签约合作）
        /// </summary>
        public int? type { get; set; }
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
