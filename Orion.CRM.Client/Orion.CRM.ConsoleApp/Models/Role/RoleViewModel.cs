using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.ConsoleApp.Models.Role
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
        /// 菜单
        /// </summary>
        public IEnumerable<Menu.MenuModel> MenuList { get; set; }
        public IEnumerable<Organization.OrganizationViewModel> OrgList { get; set; }
        public IEnumerable<Role.RoleMenu> RoleMenus { get; set; }
    }
}
