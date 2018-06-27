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
    public class RoleController : Controller
    {
        private readonly AppConfig _appConfig;
        public RoleController(IOptions<AppConfig> optionsAccessor)
        {
            _appConfig = optionsAccessor.Value;
        }

        /// <summary>
        /// 分页展示数据
        /// </summary>
        /// <param name="id">此处的id是页索引PageIndex</param>
        /// <returns></returns>
        public IActionResult RoleList(int id = 1)
        {
            ViewBag.OperateResult = Request.Query["operateResult"].ToString();

            string url = _appConfig.WebAPIHost + "api/Role/GetRoles?pageIndex=" + id + "&pageSize=" + _appConfig.PageSize;
            List<Models.Role.RoleViewModel> list = APIInvoker.Get<List<Models.Role.RoleViewModel>>(url);

            int totalCount = APIInvoker.Get<int>(_appConfig.WebAPIHost + "api/Role/GetRoleCount");

            var pageOption = new PagerOption {
                PageIndex = id,
                PageSize = _appConfig.PageSize,
                TotalCount = totalCount,
                RouteUrl = "/Role/RoleList"
            };

            //分页参数
            ViewBag.PagerOption = pageOption;

            //数据
            return View(list);
        }

        public IActionResult Create()
        {
            Models.Role.RoleViewModel viewModel = new Models.Role.RoleViewModel();
            viewModel.OrgList = APIInvoker.Get<IEnumerable<Models.Organization.OrganizationViewModel>>(_appConfig.WebAPIHost + "api/Organization/GetAllOrganizations");
            viewModel.MenuList = APIInvoker.Get<IEnumerable<Models.Menu.MenuModel>>(_appConfig.WebAPIHost + "api/MenuPage/GetAllMenus");

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CreateHandler(Models.Role.RoleViewModel viewModel)
        {
            if (viewModel != null) {
                string apiUrl = _appConfig.WebAPIHost + "api/Role/InsertRole";
                var role = new {
                    RoleName = viewModel.RoleName,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    OrgId = viewModel.OrgId
                };
                int primaryId = APIInvoker.Post<int>(apiUrl, role);

                if (primaryId > 0) {
                    // 1.获取角色和一级菜单的关系
                    List<object> roleMenuRelations = new List<object>();
                    string menuIds = Request.Form["ckRoleMenu"];
                    if (!string.IsNullOrEmpty(menuIds)) {
                        string[] menuIdArr = menuIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (menuIdArr.Length > 0) {
                            foreach (var menuId in menuIdArr) {
                                roleMenuRelations.Add(new
                                {
                                    RoleId = primaryId,
                                    MenuId = menuId,
                                    CreateTime = DateTime.Now
                                });
                            }
                        }
                    }

                    // 3.获取角色和一级菜单的关系
                    List<int> parentMenuIds = new List<int>();
                    List<Models.Menu.MenuModel> menus = APIInvoker.Get<List<Models.Menu.MenuModel>>(_appConfig.WebAPIHost + "api/MenuPage/GetAllMenus");

                    if (menus != null && menus.Count > 0) {
                        if (!string.IsNullOrEmpty(menuIds)) {
                            string[] menuIdArr = menuIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            if (menuIdArr.Length > 0) {
                                foreach (var menuId in menuIdArr) {
                                    var menu = menus.FirstOrDefault(x => x.Id == int.Parse(menuId));
                                    if (menu != null && menu.Parent != null && menu.Parent > 0) {
                                        int parentMenuId = Convert.ToInt32(menu.Parent);
                                        if (!parentMenuIds.Contains(parentMenuId)) {
                                            parentMenuIds.Add(parentMenuId);
                                        }
                                    }
                                }
                                if(parentMenuIds.Count > 0) {
                                    foreach(var parentMenuId in parentMenuIds) {
                                        roleMenuRelations.Add(new
                                        {
                                            RoleId = primaryId,
                                            MenuId = parentMenuId,
                                            CreateTime = DateTime.Now
                                        });
                                    }
                                }
                            }
                        }
                    }

                    // 插入数据库
                    if (roleMenuRelations != null && roleMenuRelations.Count > 0) {
                        string roleMenuInsertApiUrl = _appConfig.WebAPIHost + "api/Role/RoleMenuBatchInsert";
                        bool res = APIInvoker.Post<bool>(roleMenuInsertApiUrl, roleMenuRelations);
                    }
                    return RedirectToAction("RoleList", new { operateResult = "success" });
                }
                else {
                    return RedirectToAction("RoleList", new { operateResult = "fail" });
                }
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            Models.Role.RoleViewModel viewModel = new Models.Role.RoleViewModel();

            string url = _appConfig.WebAPIHost + "api/Role/GetRoleById?id=" + id;
            viewModel = APIInvoker.Get<Models.Role.RoleViewModel>(url);
            viewModel.OrgList = APIInvoker.Get<IEnumerable<Models.Organization.OrganizationViewModel>>(_appConfig.WebAPIHost + "api/Organization/GetAllOrganizations");
            viewModel.MenuList = APIInvoker.Get<IEnumerable<Models.Menu.MenuModel>>(_appConfig.WebAPIHost + "api/MenuPage/GetAllMenus");
            viewModel.RoleMenus = APIInvoker.Get<IEnumerable<Models.Role.RoleMenu>>(_appConfig.WebAPIHost + "api/Role/GetRoleMenusByRoleId?roleId=" + id);

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult EditHandler(Models.Role.RoleViewModel viewModel)
        {
            if (viewModel != null) {
                string apiUrl = _appConfig.WebAPIHost + "api/Role/UpdateRole";
                var role = new {
                    Id = viewModel.Id,
                    RoleName = viewModel.RoleName,
                    UpdateTime = DateTime.Now,
                    OrgId = viewModel.OrgId
                };
                bool result = APIInvoker.Post<bool>(apiUrl, role);

                if (result) {
                    // 0.删除旧的角色和菜单的关系
                    string deleteRoleMenuApiUrl = _appConfig.WebAPIHost + "api/Role/DeleteRoleMenuByRoleId?roleId=" + viewModel.Id;
                    APIInvoker.Get<bool>(deleteRoleMenuApiUrl);

                    // 1.获取角色和一级菜单的关系
                    List<object> roleMenuRelations = new List<object>();
                    string menuIds = Request.Form["ckRoleMenu"];
                    if (!string.IsNullOrEmpty(menuIds)) {
                        string[] menuIdArr = menuIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (menuIdArr.Length > 0) {
                            foreach (var menuId in menuIdArr) {
                                roleMenuRelations.Add(new
                                {
                                    RoleId = viewModel.Id,
                                    MenuId = menuId,
                                    CreateTime = DateTime.Now
                                });
                            }
                        }
                    }

                    // 3.获取角色和一级菜单的关系
                    List<int> parentMenuIds = new List<int>();
                    List<Models.Menu.MenuModel> menus = APIInvoker.Get<List<Models.Menu.MenuModel>>(_appConfig.WebAPIHost + "api/MenuPage/GetAllMenus");

                    if (menus != null && menus.Count > 0) {
                        if (!string.IsNullOrEmpty(menuIds)) {
                            string[] menuIdArr = menuIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            if (menuIdArr.Length > 0) {
                                foreach (var menuId in menuIdArr) {
                                    var menu = menus.FirstOrDefault(x => x.Id == int.Parse(menuId));
                                    if (menu != null && menu.Parent != null && menu.Parent > 0) {
                                        int parentMenuId = Convert.ToInt32(menu.Parent);
                                        if (!parentMenuIds.Contains(parentMenuId)) {
                                            parentMenuIds.Add(parentMenuId);
                                        }
                                    }
                                }
                                if (parentMenuIds.Count > 0) {
                                    foreach (var parentMenuId in parentMenuIds) {
                                        roleMenuRelations.Add(new
                                        {
                                            RoleId = viewModel.Id,
                                            MenuId = parentMenuId,
                                            CreateTime = DateTime.Now
                                        });
                                    }
                                }
                            }
                        }
                    }

                    // 插入数据库
                    if (roleMenuRelations != null && roleMenuRelations.Count > 0) {
                        string roleMenuInsertApiUrl = _appConfig.WebAPIHost + "api/Role/RoleMenuBatchInsert";
                        bool res = APIInvoker.Post<bool>(roleMenuInsertApiUrl, roleMenuRelations);
                    }

                    return RedirectToAction("RoleList", new { operateResult = "success" });
                }
                else {
                    return RedirectToAction("RoleList", new { operateResult = "fail" });
                }
            }
            return View();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            string url = _appConfig.WebAPIHost + "api/Role/DeleteRole?id=" + id;
            bool result = APIInvoker.Get<bool>(url);

            return result;
        }
    }
}