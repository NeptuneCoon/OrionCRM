using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.AppUser
{
    public class AppUserListViewModel
    {
        public AppUserSearchParams Params { get; set; }

        /// <summary>
        /// 业务组
        /// </summary>
        public List<Models.Group.Group> Groups { get; set; }
        /// <summary>
        /// 角色
        /// </summary>

        public List<Models.Role.Role> Roles { get; set; }

        /// <summary>
        /// 当前登录用户
        /// </summary>
        public int LoginUserId { get; set; }

        /// <summary>
        /// 查询到的当页资源列表数据
        /// </summary>
        public List<Models.AppUser.AppUserViewModel> Users { get; set; }
    }
}
