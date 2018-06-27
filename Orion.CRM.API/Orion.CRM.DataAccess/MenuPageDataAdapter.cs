using Orion.CRM.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Orion.CRM.DataAccess
{
    public class MenuPageDataAdapter : DataAdapter
    {
        #region 菜单相关操作

        public Entity.SystemMenu GetMenu(int id)
        {
            SqlParameter param = new SqlParameter("@ID", id);
            Entity.SystemMenu menu = SqlMapHelper.GetSqlMapSingleResult<Entity.SystemMenu>("MenuPageDomain", "GetMenu", param);
            return menu;
        }

        public IEnumerable<Entity.SystemMenu> GetParentMenus()
        {
            var menus = SqlMapHelper.GetSqlMapResult<Entity.SystemMenu>("MenuPageDomain", "GetParentMenus", null);
            return menus;
        }

        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Entity.SystemMenu> GetAllMenus()
        {
            var menus = SqlMapHelper.GetSqlMapResult<Entity.SystemMenu>("MenuPageDomain", "GetAllMenus", null);
            return menus;
        }

        /// <summary>
        /// 获取子菜单
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public IEnumerable<Entity.SystemMenu> GetChildMenus(int menuId)
        {
            SqlParameter param = new SqlParameter("@Parent", menuId);
            var childMenus = SqlMapHelper.GetSqlMapResult<Entity.SystemMenu>("MenuPageDomain", "GetChildMenus", param);
            return childMenus;
        }

        /// <summary>
        /// 获取所有二级菜单
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public IEnumerable<Entity.SystemMenu> GetAllLevel2Menus()
        {
            var level2Menus = SqlMapHelper.GetSqlMapResult<Entity.SystemMenu>("MenuPageDomain", "GetAllLevel2Menus");
            return level2Menus;
        }

        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public int InsertMenu(Entity.SystemMenu menu)
        {
            if (menu == null) return -1;

            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@MenuName", menu.MenuName));
            parameters.Add(new SqlParameter("@Icon", CheckNull(menu.Icon)));
            parameters.Add(new SqlParameter("@URL", CheckNull(menu.URL)));
            parameters.Add(new SqlParameter("@Description", CheckNull(menu.Description)));
            parameters.Add(new SqlParameter("@CreateTime", menu.CreateTime));
            parameters.Add(new SqlParameter("@UpdateTime", menu.UpdateTime));
            parameters.Add(new SqlParameter("@SortNo", menu.SortNo));
            parameters.Add(new SqlParameter("@Parent", CheckNull(menu.Parent)));

            SqlParameter[] parmArr = parameters.ToArray();

            int identityId = 0;
            try { 
                identityId = SqlMapHelper.ExecuteSqlMapScalar<int>("MenuPageDomain", "InsertMenu", parmArr);
            }
            catch{

            }
            return identityId;
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public bool UpdateMenu(Entity.SystemMenu menu)
        {
            if (menu == null || menu.Id <= 0) return false;

            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@Id", menu.Id));
            parameters.Add(new SqlParameter("@MenuName", menu.MenuName));
            parameters.Add(new SqlParameter("@Icon", CheckNull(menu.Icon)));
            parameters.Add(new SqlParameter("@URL", CheckNull(menu.URL)));
            parameters.Add(new SqlParameter("@Description", CheckNull(menu.Description)));
            parameters.Add(new SqlParameter("@UpdateTime", menu.UpdateTime));
            parameters.Add(new SqlParameter("@SortNo", menu.SortNo));
            parameters.Add(new SqlParameter("@Parent", CheckNull(menu.Parent)));

            SqlParameter[] parmArr = parameters.ToArray();
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("MenuPageDomain", "UpdateMenu", parmArr);

            return count > 0;
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteMenu(int id)
        {
            if (id <= 0) return false;

            SqlParameter param = new SqlParameter("@Id", id);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("MenuPageDomain", "DeleteMenu", param);
            return count > 0;
        }
        #endregion
    }
}
