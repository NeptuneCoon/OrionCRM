using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orion.CRM.WebTools;
using Orion.CRM.WebApp.App_Data;

namespace Orion.CRM.WebApp.Controllers
{
    /// <summary>
    /// 角色控制器
    /// </summary>
    public class RoleController : BaseController
    {
        public IActionResult List(int id = 1)
        {
            ViewBag.OperateResult = Request.Query["operateResult"].ToString();

            string url = _AppConfig.WebAPIHost + "api/Role/GetRolesByOrgId?pageIndex=" + id + "&pageSize=" + _AppConfig.PageSize + "&orgId=" + _AppUser.OrgId;
            List<Models.Role.RoleViewModel> list = APIInvoker.Get<List<Models.Role.RoleViewModel>>(url);

            int totalCount = APIInvoker.Get<int>(_AppConfig.WebAPIHost + "api/Role/GetRoleCountByOrgId?orgId=" + _AppUser.OrgId);

            var pageOption = new PagerOption {
                PageIndex = id,
                PageSize = _AppConfig.PageSize,
                TotalCount = totalCount,
                RouteUrl = "/Role/List"
            };

            //分页参数
            ViewBag.PagerOption = pageOption;

            //数据
            return View(list);
        }

        public IActionResult Create()
        {
            Models.Role.RoleViewModel viewModel = new Models.Role.RoleViewModel();
            viewModel.MenuList = APIInvoker.Get<IEnumerable<Models.Role.Menu>>(_AppConfig.WebAPIHost + "api/MenuPage/GetAllMenus");

            return View(viewModel);
        }

        #region CreateHandler
        [HttpPost]
        public IActionResult CreateHandler(Models.Role.RoleViewModel viewModel)
        {
            if (viewModel != null) {
                string apiUrl = _AppConfig.WebAPIHost + "api/Role/InsertRole";
                var role = new
                {
                    RoleName = viewModel.RoleName,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    OrgId = _AppUser.OrgId
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
                    List<Models.Role.Menu> menus = APIInvoker.Get<List<Models.Role.Menu>>(_AppConfig.WebAPIHost + "api/MenuPage/GetAllMenus");

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
                        string roleMenuInsertApiUrl = _AppConfig.WebAPIHost + "api/Role/RoleMenuBatchInsert";
                        bool res = APIInvoker.Post<bool>(roleMenuInsertApiUrl, roleMenuRelations);
                    }
                    TempData["result"] = true;
                    //return RedirectToAction("List", new { operateResult = "success" });
                }
                else {
                    TempData["result"] = false;
                    //return RedirectToAction("List", new { operateResult = "fail" });
                }
                return RedirectToAction("List");
            }
            return View();
        } 
        #endregion

        public IActionResult Edit(int id)
        {
            Models.Role.RoleViewModel viewModel = new Models.Role.RoleViewModel();

            string url = _AppConfig.WebAPIHost + "api/Role/GetRoleById?id=" + id;
            viewModel = APIInvoker.Get<Models.Role.RoleViewModel>(url);
            viewModel.MenuList = APIInvoker.Get<IEnumerable<Models.Role.Menu>>(_AppConfig.WebAPIHost + "api/MenuPage/GetAllMenus");
            viewModel.RoleMenus = APIInvoker.Get<IEnumerable<Models.Role.RoleMenu>>(_AppConfig.WebAPIHost + "api/Role/GetRoleMenusByRoleId?roleId=" + id);

            return View(viewModel);
        }

        #region EditHandler
        [HttpPost]
        public IActionResult EditHandler(Models.Role.RoleViewModel viewModel)
        {
            if (viewModel != null) {
                string apiUrl = _AppConfig.WebAPIHost + "api/Role/UpdateRole";
                var role = new
                {
                    Id = viewModel.Id,
                    RoleName = viewModel.RoleName,
                    UpdateTime = DateTime.Now,
                    OrgId = _AppUser.OrgId
                };
                bool result = APIInvoker.Post<bool>(apiUrl, role);

                if (result) {
                    // 0.删除旧的角色和菜单的关系
                    string deleteRoleMenuApiUrl = _AppConfig.WebAPIHost + "api/Role/DeleteRoleMenuByRoleId?roleId=" + viewModel.Id;
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
                    List<Models.Role.Menu> menus = APIInvoker.Get<List<Models.Role.Menu>>(_AppConfig.WebAPIHost + "api/MenuPage/GetAllMenus");

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
                        string roleMenuInsertApiUrl = _AppConfig.WebAPIHost + "api/Role/RoleMenuBatchInsert";
                        bool res = APIInvoker.Post<bool>(roleMenuInsertApiUrl, roleMenuRelations);
                    }
                    TempData["result"] = true;
                    //return RedirectToAction("List", new { operateResult = "success" });
                }
                else {
                    TempData["result"] = false;
                    //return RedirectToAction("List", new { operateResult = "fail" });
                }
                return RedirectToAction("List");
            }
            return View();
        } 
        #endregion
    }
}