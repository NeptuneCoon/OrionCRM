using System;
using System.Collections.Generic;
using System.Text;
using Orion.CRM.DataAccess;

namespace Orion.CRM.Application
{
    public class MenuPageAppService
    {
        private MenuPageDataAdapter adapter = new MenuPageDataAdapter();

        public int CreateMenu(Entity.SystemMenu menu)
        {
            int primaryId = adapter.InsertMenu(menu);
            return primaryId;
        }

        public Entity.SystemMenu GetMenu(int menuId)
        {
            Entity.SystemMenu menu = adapter.GetMenu(menuId);
            return menu;
        }

        public IEnumerable<Entity.SystemMenu> GetParentMenus()
        {
            IEnumerable<Entity.SystemMenu> menus = adapter.GetParentMenus();
            return menus;
        }

        public IEnumerable<Entity.SystemMenu> GetAllMenus()
        {
            IEnumerable<Entity.SystemMenu> menus = adapter.GetAllMenus();
            return menus;
        }

        public IEnumerable<Entity.SystemMenu> GetChildMenus(int menuId)
        {
            IEnumerable<Entity.SystemMenu> childMenus = adapter.GetChildMenus(menuId);
            return childMenus;
        }

        /// <summary>
        /// 获取所有二级菜单
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public IEnumerable<Entity.SystemMenu> GetAllLevel2Menus()
        {
            IEnumerable<Entity.SystemMenu> level2Menus = adapter.GetAllLevel2Menus();
            return level2Menus;
        }

        public bool UpdateMenu(Entity.SystemMenu menu)
        {
            bool result = adapter.UpdateMenu(menu);
            return result;
        }

        public bool DeleteMenu(int menuId)
        {
            // 1.获取Menu下的所有Page
            IEnumerable<Entity.SystemPage> delingPages = adapter.GetPagesByMenuId(menuId);

            // 2.获取Page和Role的关系
            bool result = adapter.DeleteMenu(menuId);
            return result;
        }

        public Entity.SystemPage GetPage(int pageId)
        {
            Entity.SystemPage page = adapter.GetPage(pageId);
            return page;
        }

        public IEnumerable<Entity.SystemPage> GetPages(int pageIndex, int pageSize)
        {
            return adapter.GetPages(pageIndex, pageSize);
        }

        public int GetPageCount()
        {
            return adapter.GetPageCount();
        }

        public IEnumerable<Entity.SystemPage> GetPagesByMenuId(int menuId)
        {
            IEnumerable<Entity.SystemPage> pages = adapter.GetPagesByMenuId(menuId);
            return pages;
        }


        public int CreatePage(Entity.SystemPage page)
        {
            int identityId = adapter.InsertPage(page);
            return identityId;
        }

        public bool UpdatePage(Entity.SystemPage page)
        {
            bool result = adapter.UpdatePage(page);
            return result;
        }

        public bool DeletePage(int pageId)
        {
            bool result = adapter.DeletePage(pageId);
            return result;
        }

        public bool PageBatchInsert(IEnumerable<Entity.SystemPage> pages)
        {
            bool result = adapter.PageBatchInsert(pages);
            return result;
        }
    }
}
