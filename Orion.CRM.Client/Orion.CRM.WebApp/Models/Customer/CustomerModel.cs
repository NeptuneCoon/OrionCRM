using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Customer
{
    public class CustomerModel
    {

        public int Id { get; set; }
        public string RealName { get; set; }
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
        public string ServiceUserName { get; set; }//售后服务专员姓名
    }
}
