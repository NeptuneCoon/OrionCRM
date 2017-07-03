using Orion.CRM.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Linq;

namespace Orion.CRM.DataAccess
{
    public class RoleDataAdapter : DataAdapter
    {
        public Entity.Role GetRoleById(int id)
        {
            SqlParameter param = new SqlParameter("@Id", id);
            Entity.Role role = SqlMapHelper.GetSqlMapSingleResult<Entity.Role>("RoleDomain", "GetRoleById", param);

            return role;
        }

        public int InsertRole(Entity.Role role)
        {
            if (role == null) return -1;

            SqlParameter[] paramArr = {
                new SqlParameter("@RoleName", role.RoleName),
                new SqlParameter("@CreateTime", role.CreateTime),
                new SqlParameter("@UpdateTime", role.UpdateTime),
                new SqlParameter("@OrgId", role.OrgId)
            };

            int identityId = SqlMapHelper.ExecuteSqlMapScalar<int>("RoleDomain", "InsertRole", paramArr);
            return identityId;
        }

        public bool UpdateRole(Entity.Role role)
        {
            if (role == null || role.Id <= 0) return false;

            SqlParameter[] paramArr = {
                new SqlParameter("@Id", role.Id),
                new SqlParameter("@RoleName", role.RoleName),
                new SqlParameter("@UpdateTime", role.UpdateTime)
            };

            int count = SqlMapHelper.ExecuteSqlMapNonQuery("RoleDomain", "UpdateRole", paramArr);
            return count > 0;
        }

        public bool DeleteRole(int id)
        {
            if (id <= 0) return false;

            SqlParameter param = new SqlParameter("@Id", id);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("RoleDomain", "DeleteRole", param);

            return count > 0;
        } 

        public int InsertRoleMenu(Entity.RoleMenu roleMenu)
        {
            if (roleMenu == null) return -1;

            SqlParameter[] paramArr = {
                new SqlParameter("@RoleId", roleMenu.RoleId),
                new SqlParameter("@MenuId", roleMenu.MenuId),
                new SqlParameter("@CreateTime", DateTime.Now)
            };

            int identityId = SqlMapHelper.ExecuteSqlMapScalar<int>("RoleDomain", "InsertRoleMenu", paramArr);
            return identityId;
        }

        public int InsertRolePage(Entity.RolePage rolePage)
        {
            if (rolePage == null) return -1;

            SqlParameter[] paramArr = {
                new SqlParameter("@RoleId", rolePage.RoleId),
                new SqlParameter("@PageId", rolePage.PageId),
                new SqlParameter("@CreateTime", DateTime.Now)
            };

            int identityId = SqlMapHelper.ExecuteSqlMapScalar<int>("RoleDomain", "InsertRolePage", paramArr);
            return identityId;
        }

        public bool DeleteRoleMenuById(int id)
        {
            if (id <= 0) return false;

            SqlParameter param = new SqlParameter("@Id", id);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("RoleDomain", "DeleteRoleMenuById", param);

            return count > 0;
        }

        public int DeleteRoleMenuByRoleId(int roleId)
        {
            if (roleId <= 0) return 0;

            SqlParameter param = new SqlParameter("@RoleId", roleId);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("RoleDomain", "DeleteRoleMenuByRoleId", param);

            return count;
        }

        public bool DeleteRoleMenuByMenuId(int menuId)
        {
            if (menuId <= 0) return false;

            SqlParameter param = new SqlParameter("@MenuId", menuId);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("RoleDomain", "DeleteRoleMenuByMenuId", param);

            return count > 0;
        }

        public bool DeleteRolePageByRoleId(int roleId)
        {
            if (roleId <= 0) return false;

            SqlParameter param = new SqlParameter("@RoleId", roleId);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("RoleDomain", "DeleteRolePageByRoleId", param);

            return count > 0;
        }

        public bool DeleteRolePageByPageId(int pageId)
        {
            if (pageId <= 0) return false;

            SqlParameter param = new SqlParameter("@PageId", pageId);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("RoleDomain", "DeleteRolePageByPageId", param);

            return count > 0;
        }

        public IEnumerable<Entity.Role> GetRoles(int pageIndex, int pageSize)
        {
            SqlMapDetail mapDetail = (SqlMapDetail)SqlMapFactory.GetSqlMapDetail("RoleDomain", "GetRoles").Clone();
            mapDetail.OriginalSqlString = mapDetail.OriginalSqlString.Replace("$PageIndex", pageIndex.ToString()).Replace("$PageSize", pageSize.ToString());

            IEnumerable<Entity.Role> roles = SqlMapHelper.GetSqlMapResult<Entity.Role>(mapDetail);
            return roles;
        }

        public int GetRoleCount()
        {
            int count = SqlMapHelper.ExecuteSqlMapScalar<int>("RoleDomain", "GetRoleCount");
            return count;
        }

        public IEnumerable<Entity.Role> GetRolesByOrgId(int pageIndex, int pageSize, int orgId)
        {
            SqlMapDetail mapDetail = (SqlMapDetail)SqlMapFactory.GetSqlMapDetail("RoleDomain", "GetRolesByOrgId").Clone();
            mapDetail.OriginalSqlString = mapDetail.OriginalSqlString.Replace("$PageIndex", pageIndex.ToString()).Replace("$PageSize", pageSize.ToString());
            mapDetail.OriginalSqlString = mapDetail.OriginalSqlString.Replace("$OrgId", orgId.ToString());

            IEnumerable<Entity.Role> roles = SqlMapHelper.GetSqlMapResult<Entity.Role>(mapDetail);
            return roles;
        }

        public int GetRoleCountByOrgId(int orgId)
        {
            SqlParameter param = new SqlParameter("@OrgId", orgId);
            int count = SqlMapHelper.ExecuteSqlMapScalar<int>("RoleDomain", "GetRoleCountByOrgId", param);
            return count;
        }

        public IEnumerable<Entity.RoleMenu> GetRoleMenusByRoleId(int roleId)
        {
            if (roleId <= 0) return null;

            SqlParameter param = new SqlParameter("@RoleId", roleId);
            IEnumerable<Entity.RoleMenu> roleMenus = SqlMapHelper.GetSqlMapResult<Entity.RoleMenu>("RoleDomain", "GetRoleMenusByRoleId", param);

            return roleMenus;
        }

        public IEnumerable<Entity.RoleMenu> GetRoleMenusByMenuId(int menuId)
        {
            if (menuId <= 0) return null;

            SqlParameter param = new SqlParameter("@MenuId", menuId);
            IEnumerable<Entity.RoleMenu> roleMenus = SqlMapHelper.GetSqlMapResult<Entity.RoleMenu>("RoleDomain", "GetRoleMenusByMenuId", param);

            return roleMenus;
        }

        public IEnumerable<Entity.RolePage> GetRolePagesByRoleId(int roleId)
        {
            if (roleId <= 0) return null;

            SqlParameter param = new SqlParameter("@RoleId", roleId);
            IEnumerable<Entity.RolePage> rolePages = SqlMapHelper.GetSqlMapResult<Entity.RolePage>("RoleDomain", "GetRolePagesByRoleId", param);

            return rolePages;
        }

        public IEnumerable<Entity.RoleMenu> GetRoleMenusByOrgId(int orgId)
        {
            if (orgId <= 0) return null;

            SqlParameter param = new SqlParameter("@OrgId", orgId);
            IEnumerable<Entity.RoleMenu> roleMenus = SqlMapHelper.GetSqlMapResult<Entity.RoleMenu>("RoleDomain", "GetRoleMenusByOrgId", param);

            return roleMenus;
        }

        public IEnumerable<Entity.RolePage> GetRolePagesByPageId(int pageId)
        {
            if (pageId <= 0) return null;

            SqlParameter param = new SqlParameter("@PageId", pageId);
            IEnumerable<Entity.RolePage> rolePages = SqlMapHelper.GetSqlMapResult<Entity.RolePage>("RoleDomain", "GetRolePagesByPageId", param);

            return rolePages;
        }


        public IEnumerable<Entity.RoleMenuComplex> GetAllRoleMenus()
        {
            IEnumerable<Entity.RoleMenuComplex> roleMenuComplexs = SqlMapHelper.GetSqlMapResult<Entity.RoleMenuComplex>("RoleDomain", "GetAllRoleMenus");

            return roleMenuComplexs;
        }

        public IEnumerable<Entity.RoleMenuComplex> GetComplexRoleMenusByRoleId(int roleId)
        {
            SqlParameter param = new SqlParameter("@RoleId", roleId);
            IEnumerable<Entity.RoleMenuComplex> roleMenuComplexs = SqlMapHelper.GetSqlMapResult<Entity.RoleMenuComplex>("RoleDomain", "GetComplexRoleMenusByRoleId", param);

            return roleMenuComplexs;
        }

        public IEnumerable<Entity.RoleMenuComplex> GetAllComplexRoleMenus()
        {
            IEnumerable<Entity.RoleMenuComplex> roleMenuComplexs = SqlMapHelper.GetSqlMapResult<Entity.RoleMenuComplex>("RoleDomain", "GetAllComplexRoleMenus");
            return roleMenuComplexs;
        }

        public bool RoleMenuBatchInsert(IEnumerable<Entity.RoleMenu> roleMenus)
        {
            bool result = SqlMapHelper.ExecuteBatchInsert<Entity.RoleMenu>("RoleDomain", "RoleMenuBatchInsert", roleMenus);
            return result;
        }
    }
}
