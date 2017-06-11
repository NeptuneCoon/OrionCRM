using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Orion.CRM.WebTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Orion.CRM.ConsoleApp.Controllers
{
    public class MenuController : Controller
    {
        private readonly AppConfig _appConfig;
        public MenuController(IOptions<AppConfig> optionsAccessor)
        {
            _appConfig = optionsAccessor.Value;
        }

        public IActionResult MenuList()
        {
            string url = _appConfig.WebAPIHost + "api/MenuPage/GetAllMenus";
            List<Models.Menu.MenuModel> list = APIInvoker.Get<List<Models.Menu.MenuModel>>(url);

            ViewBag.OperateResult = Request.Query["operateResult"].ToString(); 
            return View(list);
        }

        public IActionResult Create()
        {
            Models.Menu.MenuEditViewModel viewModel = new Models.Menu.MenuEditViewModel();
            // 菜单类型，1=一级菜单，2=二级菜单
            viewModel.Type = Convert.ToInt32(Request.Query["type"]);

            // 如果要编辑的是二级菜单，则要拉取一级菜单列表
            if(viewModel.Type == 2) {
                string url = _appConfig.WebAPIHost + "api/MenuPage/GetParentMenus";
                viewModel.ParentMenus = APIInvoker.Get<List<Models.Menu.MenuModel>>(url);
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CreateHandler(Models.Menu.MenuEditViewModel viewModel)
        {
            if (viewModel != null) {
                string apiUrl = _appConfig.WebAPIHost + "api/MenuPage/CreateMenu";
                var menu = new {
                    MenuName = viewModel.Menu.MenuName,
                    Description = viewModel.Menu.Description,
                    Icon = viewModel.Menu.Icon,
                    URL = viewModel.Menu.URL,
                    SortNo = viewModel.Menu.SortNo,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    Parent = viewModel.Menu.Parent
                };

                int primaryId = APIInvoker.Post<int>(apiUrl, menu);

                if (primaryId > 0) {
                    return RedirectToAction("MenuList", new { operateResult = "success" });
                }
                else {
                    return RedirectToAction("MenuList", new { operateResult = "fail" });
                }
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            Models.Menu.MenuEditViewModel viewModel = new Models.Menu.MenuEditViewModel();
            
            string url = _appConfig.WebAPIHost + "api/MenuPage/GetMenu?id=" + id;
            viewModel.Menu = APIInvoker.Get<Models.Menu.MenuModel>(url);
            if(viewModel.Menu.Parent == null || viewModel.Menu.Parent == 0) {
                viewModel.Type = 1;
            }
            else {
                viewModel.Type = 2;
            }

            // 如果要编辑的是二级菜单，则要拉取一级菜单列表
            if (viewModel.Type == 2) {
                url = _appConfig.WebAPIHost + "api/MenuPage/GetParentMenus";
                viewModel.ParentMenus = APIInvoker.Get<List<Models.Menu.MenuModel>>(url);
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult EditHandler(Models.Menu.MenuEditViewModel viewModel)
        {
            if (viewModel != null) {
                string apiUrl = _appConfig.WebAPIHost + "api/MenuPage/UpdateMenu";
                var menu = new {
                    Id = viewModel.Menu.Id,
                    MenuName = viewModel.Menu.MenuName,
                    Description = viewModel.Menu.Description,
                    Icon = viewModel.Menu.Icon,
                    URL = viewModel.Menu.URL,
                    SortNo = viewModel.Menu.SortNo,
                    UpdateTime = DateTime.Now,
                    Parent = viewModel.Menu.Parent
                };

                bool result = APIInvoker.Post<bool>(apiUrl, menu);

                if (result) {
                    return RedirectToAction("MenuList", new { operateResult = "success" });
                }
                else {
                    return RedirectToAction("MenuList", new { operateResult = "fail" });
                }
            }
            return View();
        }

        public bool DeleteMenu(int id)
        {
            string apiUrl = _appConfig.WebAPIHost + "api/MenuPage/DeleteMenu?menuId=" + id;
            bool result = APIInvoker.Get<bool>(apiUrl);

            return result;
        }
    }
}