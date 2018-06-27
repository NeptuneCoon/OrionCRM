using Orion.CRM.WebApp.Models.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Role
{
    public class RoleViewModel
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public int OrgId { get; set; }
        public string OrgName { get; set; }
        /// <summary>
        /// 角色下的用户数
        /// </summary>
        public int UserCount { get; set; }
        /// <summary>
        /// 菜单
        /// </summary>
        public IEnumerable<Menu> MenuList { get; set; }
        public IEnumerable<OrganizationModel> OrgList { get; set; }
        public IEnumerable<RoleMenu> RoleMenus { get; set; }
        public IEnumerable<RoleDataPermission> RolePermissions { get; set; }
    }
}
