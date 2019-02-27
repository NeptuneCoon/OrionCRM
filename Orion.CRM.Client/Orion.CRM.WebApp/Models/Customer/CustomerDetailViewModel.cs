using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Customer
{
    public class CustomerDetailViewModel
    {
        /// <summary>
        /// 客户
        /// </summary>
        public Models.Customer.CustomerModel Customer { get; set; }

        /// <summary>
        /// 客户的服务记录
        /// </summary>
        public  List<Models.Customer.CustomerServiceRecord> ServiceRecords { get; set; }
        
    }
}
