using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Sign
{
    /// <summary>
    /// 签约记录
    /// </summary>
    public class CustomerSign
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public int ResourceId { get; set; }
        public DateTime SignTime { get; set; }
        public int SignUserId { get; set; }
        public string SignMan { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int ProjectId { get; set; }
        public int OrgId { get; set; }
        public DateTime CreateTime { get; set; }
        public int AppendUserId { get; set; }
        public string AppendMan { get; set; }

        /// <summary>
        /// 客户姓名
        /// </summary>
        public string CustomerName { get; set; }
    }
}
