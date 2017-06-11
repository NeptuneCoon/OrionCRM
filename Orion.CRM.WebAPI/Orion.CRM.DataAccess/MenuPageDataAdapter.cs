using Orion.CRM.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Orion.CRM.DataAccess
{
    public class MenuPageDataAdapter
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
            if(menu.Icon != null) { 
                parameters.Add(new SqlParameter("@Icon", menu.Icon));
            }
            else {
                parameters.Add(new SqlParameter("@Icon", DBNull.Value));
            }
            if (menu.URL != null) {
                parameters.Add(new SqlParameter("@URL", menu.URL));
            }
            else {
                parameters.Add(new SqlParameter("@URL", DBNull.Value));
            }
            if (menu.Description != null) { 
                parameters.Add(new SqlParameter("@Description", menu.Description));
            }
            else {
                parameters.Add(new SqlParameter("@Description", DBNull.Value));
            }
            parameters.Add(new SqlParameter("@CreateTime", menu.CreateTime));
            parameters.Add(new SqlParameter("@UpdateTime", menu.UpdateTime));
            parameters.Add(new SqlParameter("@SortNo", menu.SortNo));
            if (menu.Parent != null) {
                parameters.Add(new SqlParameter("@Parent", menu.Parent));
            }
            else {
                parameters.Add(new SqlParameter("@Parent", DBNull.Value));
            }

            SqlParameter[] parmArr = parameters.ToArray();

            int identityId = 0;
            try { 
                identityId = SqlMapHelper.ExecuteSqlMapScalar<int>("MenuPageDomain", "InsertMenu", parmArr);
            }
            catch(Exception ex) {

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
            if (menu.Icon != null) {
                parameters.Add(new SqlParameter("@Icon", menu.Icon));
            }
            else {
                parameters.Add(new SqlParameter("@Icon", DBNull.Value));
            }
            if (menu.URL != null) {
                parameters.Add(new SqlParameter("@URL", menu.URL));
            }
            else {
                parameters.Add(new SqlParameter("@URL", DBNull.Value));
            }
            if (menu.Description != null) {
                parameters.Add(new SqlParameter("@Description", menu.Description));
            }
            else {
                parameters.Add(new SqlParameter("@Description", DBNull.Value));
            }
            parameters.Add(new SqlParameter("@UpdateTime", menu.UpdateTime));
            parameters.Add(new SqlParameter("@SortNo", menu.SortNo));
            if (menu.Parent != null) {
                parameters.Add(new SqlParameter("@Parent", menu.Parent));
            }
            else {
                parameters.Add(new SqlParameter("@Parent", DBNull.Value));
            }

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
            int count = 0;
            try {
                count = SqlMapHelper.ExecuteSqlMapNonQuery("MenuPageDomain", "DeleteMenu", param);
            }
            catch(Exception ex) {
                // log..
            }

            return count > 0;
        }
        #endregion


        #region 页面相关操作
        public Entity.SystemPage GetPage(int id)
        {
            SqlParameter param = new SqlParameter("@ID", id);
            Entity.SystemPage page = SqlMapHelper.GetSqlMapSingleResult<Entity.SystemPage>("MenuPageDomain", "GetPage", param);
            return page;
        }

        public IEnumerable<Entity.SystemPage> GetPages(int pageIndex, int pageSize)
        {
            SqlMapDetail mapDetail = (SqlMapDetail)SqlMapFactory.GetSqlMapDetail("MenuPageDomain", "GetPages").Clone();
            mapDetail.OriginalSqlString = mapDetail.OriginalSqlString.Replace("$PageIndex", pageIndex.ToString()).Replace("$PageSize", pageSize.ToString());

            IEnumerable<Entity.SystemPage> pages = SqlMapHelper.GetSqlMapResult<Entity.SystemPage>(mapDetail);
            return pages;
        }

        public int GetPageCount()
        {
            int totalCount = SqlMapHelper.ExecuteSqlMapScalar<int>("MenuPageDomain", "GetPageCount");
            return totalCount;
        }

        public IEnumerable<Entity.SystemPage> GetPagesByMenuId(int menuId)
        {
            SqlParameter param = new SqlParameter("@MenuId", menuId);
            var pages = SqlMapHelper.GetSqlMapResult<Entity.SystemPage>("MenuPageDomain", "GetPagesByMenuId", param);
            return pages;
        }

        public int InsertPage(Entity.SystemPage page)
        {
            if (page == null) return -1;

            SqlParameter[] paramArr = {
                new SqlParameter("@PageName", page.PageName),
                new SqlParameter("@PageURL", page.PageURL),
                new SqlParameter("@MenuId", page.MenuId),
                new SqlParameter("@CreateTime", page.CreateTime),
                new SqlParameter("@UpdateTime", page.UpdateTime),
                new SqlParameter("@DefaultPage", page.DefaultPage)
            };

            int identityId = SqlMapHelper.ExecuteSqlMapScalar<int>("MenuPageDomain", "InsertPage", paramArr);
            return identityId;
        }

        public bool UpdatePage(Entity.SystemPage page)
        {
            if (page == null || page.Id <= 0) return false;

            SqlParameter[] paramArr = {
                new SqlParameter("@Id", page.Id),
                new SqlParameter("@PageName", page.PageName),
                new SqlParameter("@PageURL", page.PageURL),
                new SqlParameter("@MenuId", page.MenuId),
                new SqlParameter("@UpdateTime", page.UpdateTime),
                new SqlParameter("@DefaultPage", page.DefaultPage)
            };

            int count = SqlMapHelper.ExecuteSqlMapNonQuery("MenuPageDomain", "UpdatePage", paramArr);
            return count > 0;
        }

        public bool DeletePage(int id)
        {
            if (id <= 0) return false;

            SqlParameter param = new SqlParameter("@Id", id);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("MenuPageDomain", "DeletePage", param);

            return count > 0;
        }

        public bool PageBatchInsert(IEnumerable<Entity.SystemPage> pages)
        {
            bool result = SqlMapHelper.ExecuteBatchInsert<Entity.SystemPage>("MenuPageDomain", "PageBatchInsert", pages);
            return result;
        }
        #endregion
    }
}
