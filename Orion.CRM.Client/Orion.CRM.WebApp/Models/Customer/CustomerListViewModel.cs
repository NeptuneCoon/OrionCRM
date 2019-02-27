using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Customer
{
    /// <summary>
    /// 客户数据列表视图模型
    /// </summary>
    public class CustomerListViewModel
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        public CustomerSearchParams Params { get; set; }

        /// <summary>
        /// 数据列表
        /// </summary>
        public List<Models.Customer.CustomerModel> Customers { get; set; }

        /// <summary>
        /// 当前组织机构下的用户
        /// </summary>
        public List<Models.AppUser.AppUserComplex> OrgUsers { get; set; }


        public List<Models.Project.Project> ProjectList { get; set; }
    }
}
