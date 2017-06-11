using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orion.CRM.Application;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Orion.CRM.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class MenuPageController : Controller
    { 
        private MenuPageAppService service = new MenuPageAppService();

        #region 菜单相关操作
        public APIDataResult GetMenu(int id)
        {
            try {
                Entity.SystemMenu menu = service.GetMenu(id);
                APIDataResult dataResult = new APIDataResult(200, menu);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult GetParentMenus()
        {
            try {
                IEnumerable<Entity.SystemMenu> menus = service.GetParentMenus();
                APIDataResult dataResult = new APIDataResult(200, menus);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult GetAllMenus()
        {
            try {
                IEnumerable<Entity.SystemMenu> menus = service.GetAllMenus();
                APIDataResult dataResult = new APIDataResult(200, menus);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        //[HttpGet("{menuId}")]
        public APIDataResult GetChildMenus(int menuId)
        {
            try {
                IEnumerable<Entity.SystemMenu> childMenus = service.GetChildMenus(menuId);
                APIDataResult dataResult = new APIDataResult(200, childMenus);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult GetAllLevel2Menus()
        {
            try {
                IEnumerable<Entity.SystemMenu> level2Menus = service.GetAllLevel2Menus();
                APIDataResult dataResult = new APIDataResult(200, level2Menus);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult CreateMenu([FromBody]Entity.SystemMenu menu)
        {
            try {
                int primaryId = service.CreateMenu(menu);
                APIDataResult dataResult = new APIDataResult(200, primaryId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }


        [HttpPost]
        public APIDataResult UpdateMenu([FromBody]Entity.SystemMenu menu)
        {
            try {
                bool res = service.UpdateMenu(menu);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult DeleteMenu(int menuId)
        {
            try {
                bool res = service.DeleteMenu(menuId);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }
        #endregion


        #region 页面相关操作
        public APIDataResult GetPage(int pageId)
        {
            try {
                Entity.SystemPage page = service.GetPage(pageId);
                APIDataResult dataResult = new APIDataResult(200, page);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult GetPages([FromQuery]int pageIndex, int pageSize)
        {
            try {
                IEnumerable<Entity.SystemPage> pages = service.GetPages(pageIndex, pageSize);
                APIDataResult dataResult = new APIDataResult(200, pages);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult GetPageCount()
        {
            try {
                int count = service.GetPageCount();
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult GetPagesByMenuId(int menuId)
        {
            try {
                IEnumerable<Entity.SystemPage> pages = service.GetPagesByMenuId(menuId);
                APIDataResult dataResult = new APIDataResult(200, pages);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult CreatePage([FromBody]Entity.SystemPage page)
        {
            try {
                int identityId = service.CreatePage(page);
                APIDataResult dataResult = new APIDataResult(200, identityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult UpdatePage([FromBody]Entity.SystemPage page)
        {
            try {
                bool res = service.UpdatePage(page);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult DeletePage(int pageId)
        {
            try {
                bool res = service.DeletePage(pageId);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult PageBatchInsert([FromBody]IEnumerable<Entity.SystemPage> pages)
        {
            try {
                bool res = service.PageBatchInsert(pages);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        } 
        #endregion
    }
}
