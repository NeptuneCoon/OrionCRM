using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Orion.CRM.WebTools;
using Newtonsoft.Json;

namespace Orion.CRM.ConsoleApp.Controllers
{
    public class PageController : Controller
    {
        private readonly AppConfig _appConfig;
        public PageController(IOptions<AppConfig> optionsAccessor)
        {
            _appConfig = optionsAccessor.Value;
        }

        /// <summary>
        /// 分页展示数据
        /// </summary>
        /// <param name="id">此处的id是页索引PageIndex</param>
        /// <returns></returns>
        public IActionResult PageList(int id = 1)
        {
            ViewBag.OperateResult = Request.Query["operateResult"].ToString();

            string url = _appConfig.WebAPIHost + "api/MenuPage/GetPages?pageIndex=" + id + "&pageSize=" + _appConfig.PageSize;
            List<Models.Page.PageViewModel> list = APIInvoker.Get<List<Models.Page.PageViewModel>>(url);

            int totalCount = APIInvoker.Get<int>(_appConfig.WebAPIHost + "api/MenuPage/GetPageCount");

            var pageOption = new PagerOption {
                PageIndex = id,
                PageSize = _appConfig.PageSize,
                TotalCount = totalCount,
                RouteUrl = "/Page/PageList"
            };

            //分页参数
            ViewBag.PagerOption = pageOption;

            //数据
            return View(list);
        }

        public IActionResult Create()
        {
            Models.Page.PageViewModel viewModel = new Models.Page.PageViewModel();
            viewModel.Level2Menus = APIInvoker.Get<List<Models.Menu.MenuModel>>(_appConfig.WebAPIHost + "api/MenuPage/GetAllLevel2Menus");
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CreateHandler(Models.Page.PageViewModel viewModel)
        {
            if (viewModel != null) {
                string apiUrl = _appConfig.WebAPIHost + "api/MenuPage/CreatePage";
                var org = new {
                    PageName = viewModel.PageName,
                    PageURL = viewModel.PageURL,
                    MenuId = viewModel.MenuId,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    DefaultPage = viewModel.DefaultPage
                };
                int primaryId = APIInvoker.Post<int>(apiUrl, org);

                if (primaryId > 0) {
                    return RedirectToAction("PageList", new { operateResult = "success" });
                }
                else {
                    return RedirectToAction("PageList", new { operateResult = "fail" });
                }
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            Models.Page.PageViewModel viewModel = new Models.Page.PageViewModel();

            string url = _appConfig.WebAPIHost + "api/MenuPage/GetPage?pageId=" + id;
            viewModel = APIInvoker.Get<Models.Page.PageViewModel>(url);
            viewModel.Level2Menus = APIInvoker.Get<List<Models.Menu.MenuModel>>(_appConfig.WebAPIHost + "api/MenuPage/GetAllLevel2Menus");

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult EditHandler(Models.Page.PageViewModel viewModel)
        {
            if (viewModel != null) {
                string apiUrl = _appConfig.WebAPIHost + "api/MenuPage/UpdatePage";
                var org = new {
                    Id = viewModel.Id,
                    PageName = viewModel.PageName,
                    PageURL = viewModel.PageURL,
                    UpdateTime = DateTime.Now,
                    MenuId = viewModel.MenuId,
                    DefaultPage = viewModel.DefaultPage
                };
                bool result = APIInvoker.Post<bool>(apiUrl, org);

                if (result) {
                    return RedirectToAction("PageList", new { operateResult = "success" });
                }
                else {
                    return RedirectToAction("PageList", new { operateResult = "fail" });
                }
            }
            return View();
        }

        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeletePage(int id)
        {
            // 获取
            string getURL = _appConfig.WebAPIHost + "api/MenuPage/DeletePage?pageId=" + id;
            bool result = APIInvoker.Get<bool>(getURL);

            return result;
        }
    }
}