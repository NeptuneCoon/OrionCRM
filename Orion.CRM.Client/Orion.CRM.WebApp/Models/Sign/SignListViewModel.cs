using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Sign
{
    public class SignListViewModel
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        public SignSearchParams Params { get; set; }

        /// <summary>
        /// 查询到的签约记录数据
        /// </summary>
        public List<Models.Sign.CustomerSign> Signs { get; set; }

        /// <summary>
        /// 当前组织机构下的用户
        /// </summary>
        public List<Models.AppUser.AppUserComplex> OrgUsers { get; set; }

        /// <summary>
        /// 组织机构下的项目
        /// </summary>
        public List<Models.Project.Project> ProjectList { get; set; }
    }
}
