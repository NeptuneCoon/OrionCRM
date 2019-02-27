using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Customer
{
    public class CustomerServiceRecord
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string ServiceContent { get; set; }
        public string Images { get; set; }
        public int AppendUserId { get; set; }
        public DateTime CreateTime { get; set; }

        // 扩展属性
        public string CustomerRealName { get; set; }// 用户真实姓名
        public string AppendUserRealName { get; set; } // 添加人姓名
    }
}
