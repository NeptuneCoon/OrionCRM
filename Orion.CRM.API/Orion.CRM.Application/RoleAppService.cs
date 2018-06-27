using System;
using System.Collections.Generic;
using System.Text;
using Orion.CRM.DataAccess;
using System.Linq;

namespace Orion.CRM.Application
{
    public class RoleAppService
    {
        private RoleDataAdapter adapter = new RoleDataAdapter();

        public Entity.Role GetRoleById(int id)
        {
            return adapter.GetRoleById(id);
        }

        public int InsertRole(Entity.Role role)
        {
            return adapter.InsertRole(role);
        }

        public bool UpdateRole(Entity.Role role)
        {
            return adapter.UpdateRole(role);
        }

        public bool DeleteRole(int id)
        {
            return adapter.DeleteRole(id);
        }

        public int InsertRoleMenu(Entity.RoleMenu roleMenu)
        {
            return adapter.InsertRoleMenu(roleMenu);
        }

        public bool InsertRoleMenuAndParentMenu()
        {


            return true;
        }

        public bool DeleteRoleMenuById(int id)
        {
            return adapter.DeleteRoleMenuById(id);
        }

        public int DeleteRoleMenuByRoleId(int roleId)
        {
            return adapter.DeleteRoleMenuByRoleId(roleId);
        }

        public bool DeleteRoleMenuByMenuId(int menuId)
        {
            return adapter.DeleteRoleMenuByMenuId(menuId);
        }

        public IEnumerable<Entity.Role> GetRoles(int pageIndex, int pageSize)
        {
            return adapter.GetRoles(pageIndex, pageSize);
        }

        public int GetRoleCount()
        {
            return adapter.GetRoleCount();
        }

        public IEnumerable<Entity.Role> GetRolesByOrgId(int pageIndex, int pageSize, int orgId)
        {
            return adapter.GetRolesByOrgId(pageIndex, pageSize, orgId);
        }

        public int GetRoleCountByOrgId(int orgId)
        {
            return adapter.GetRoleCountByOrgId(orgId);
        }

        public IEnumerable<Entity.RoleMenu> GetRoleMenusByRoleId(int roleId)
        {
            return adapter.GetRoleMenusByRoleId(roleId);
        }

        public IEnumerable<Entity.RoleMenu> GetRoleMenusByMenuId(int menuId)
        {
            return adapter.GetRoleMenusByMenuId(menuId);
        }

        public IEnumerable<Entity.RoleMenu> GetRoleMenusByOrgId(int orgId)
        {
            return adapter.GetRoleMenusByOrgId(orgId);
        }

        public IEnumerable<Entity.RoleMenuComplex> GetAllRoleMenus()
        {
            return adapter.GetAllRoleMenus();
        }

        /// <summary>
        /// 获取角色下所有菜单
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns></returns>
        public IEnumerable<Entity.RoleMenuComplex> GetComplexRoleMenusByRoleId(int roleId)
        {
            IEnumerable<Entity.RoleMenuComplex> roleMenus = adapter.GetComplexRoleMenusByRoleId(roleId);
            // 为父级菜单的URL填充值
            if (roleMenus != null) {
                IEnumerable<Entity.RoleMenuComplex> parentMenus = roleMenus.Where(x => x.Parent == null);
                foreach(var parentMenu in parentMenus) {
                    var firstChild = roleMenus.Where(x => x.Parent == parentMenu.MenuId).OrderBy(x => x.SortNo).FirstOrDefault();
                    if (firstChild != null) {
                        parentMenu.URL = firstChild.URL;
                    }
                }
            }
            return roleMenus;
        }

        public IEnumerable<Entity.RoleMenuComplex> GetAllComplexRoleMenus()
        {
            return adapter.GetAllComplexRoleMenus();
        }

        public bool RoleMenuBatchInsert(IEnumerable<Entity.RoleMenu> roleMenus)
        {
            return adapter.RoleMenuBatchInsert(roleMenus);
        }

        public int GetUserCountByRoleId(int roleId)
        {
            return adapter.GetUserCountByRoleId(roleId);
        }
    }
}
