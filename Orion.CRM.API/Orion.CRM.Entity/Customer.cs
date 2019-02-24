using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    public class Customer
    {
        public int Id { get; set; }
        public string RealName { get; set; }
        public int? Sex { get; set; }
        public string Mobile { get; set; }
        public string IdentityNo { get; set; }
        public int BrandId { get; set; }
        /// <summary>
        /// 代理级别：1=省，2=市，3=县/行政区
        /// </summary>
        public int AgentLevel { get; set; }
        public string AgentZone1 { get; set; }
        public string AgentZone2 { get; set; }
        public string AgentZone3 { get; set; }
        public int ProjectId { get; set; }
        public int? ServiceUserId { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
