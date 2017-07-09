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
    /// ��ɫ������
    /// </summary>
    public class RoleController : BaseController
    {
        public IActionResult List(int pi = 1)
        {
            ViewBag.OperateResult = Request.Query["operateResult"].ToString();

            string url = _AppConfig.WebApiHost + "api/Role/GetRolesByOrgId?pageIndex=" + pi + "&pageSize=" + _AppConfig.PageSize + "&orgId=" + _AppUser.OrgId;
            List<Models.Role.RoleViewModel> list = APIInvoker.Get<List<Models.Role.RoleViewModel>>(url);

            int totalCount = APIInvoker.Get<int>(_AppConfig.WebApiHost + "api/Role/GetRoleCountByOrgId?orgId=" + _AppUser.OrgId);

            var pageOption = new PagerOption {
                PageIndex = pi,
                PageSize = _AppConfig.PageSize,
                TotalCount = totalCount,
                RouteUrl = "/Role/List"
            };

            //��ҳ����
            ViewBag.PagerOption = pageOption;

            //����
            return View(list);
        }

        public IActionResult Create()
        {
            Models.Role.RoleViewModel viewModel = new Models.Role.RoleViewModel();
            viewModel.MenuList = APIInvoker.Get<IEnumerable<Models.Role.Menu>>(_AppConfig.WebApiHost + "api/MenuPage/GetAllMenus");

            return View(viewModel);
        }

        #region CreateHandler
        [HttpPost]
        public IActionResult CreateHandler(Models.Role.RoleViewModel viewModel)
        {
            if (viewModel != null) {
                string apiUrl = _AppConfig.WebApiHost + "api/Role/InsertRole";
                var role = new
                {
                    RoleName = viewModel.RoleName.Trim(),
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    OrgId = _AppUser.OrgId
                };
                int primaryId = APIInvoker.Post<int>(apiUrl, role);

                if (primaryId > 0) {
                    // 1.1��ȡ��ɫ�Ͷ����˵��Ĺ�ϵ
                    List<object> roleMenuRelations = new List<object>();
                    string menuIds = Request.Form["RoleMenu"];
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

                    // 1.2��ȡ��ɫ��һ���˵��Ĺ�ϵ
                    List<int> parentMenuIds = new List<int>();
                    List<Models.Role.Menu> menus = APIInvoker.Get<List<Models.Role.Menu>>(_AppConfig.WebApiHost + "api/MenuPage/GetAllMenus");

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

                    // 1.3�������ݿ�
                    if (roleMenuRelations != null && roleMenuRelations.Count > 0) {
                        string roleMenuInsertApiUrl = _AppConfig.WebApiHost + "api/Role/RoleMenuBatchInsert";
                        bool res = APIInvoker.Post<bool>(roleMenuInsertApiUrl, roleMenuRelations);
                    }

                    // 2.2��ȡ��ɫ������Ȩ�޵Ĺ�ϵ
                    List<object> rolePermissions = new List<object>();
                    // 2.2.1��ȡ��Դ�ɼ���Χ
                    rolePermissions.Add(new
                    {
                        RoleId = viewModel.Id,
                        PermissionCategoryId = 1,
                        PermissionId = Convert.ToInt32(Request.Form["ResourceVisible"]),
                        CreateTime = DateTime.Now
                    });
                    // 2.2.2��ȡ��Դ����Ȩ��
                    string permissionIds = Request.Form["ResourceHandle"];
                    if (!string.IsNullOrEmpty(permissionIds)) {
                        string[] permissionIdArr = permissionIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (permissionIdArr.Length > 0) {
                            foreach (var permissionId in permissionIdArr) {
                                rolePermissions.Add(new
                                {
                                    RoleId = viewModel.Id,
                                    PermissionCategoryId = 2,
                                    PermissionId = permissionId,
                                    CreateTime = DateTime.Now
                                });
                            }
                        }
                    }

                    // 2.3�������ݿ�
                    if (rolePermissions != null && rolePermissions.Count > 0) {
                        string rolePermissionInsertApiUrl = _AppConfig.WebApiHost + "api/DataPermission/RoleDataPermissionBatchInsert";
                        bool rpInsertResult = APIInvoker.Post<bool>(rolePermissionInsertApiUrl, rolePermissions);
                    }

                    TempData["result"] = true;
                }
                else {
                    TempData["result"] = false;
                }
                return RedirectToAction("List");
            }
            return View();
        } 
        #endregion

        public IActionResult Edit(int id)
        {
            Models.Role.RoleViewModel viewModel = new Models.Role.RoleViewModel();

            string url = _AppConfig.WebApiHost + "api/Role/GetRoleById?id=" + id;
            viewModel = APIInvoker.Get<Models.Role.RoleViewModel>(url);
            viewModel.MenuList = APIInvoker.Get<IEnumerable<Models.Role.Menu>>(_AppConfig.WebApiHost + "api/MenuPage/GetAllMenus");
            viewModel.RoleMenus = APIInvoker.Get<IEnumerable<Models.Role.RoleMenu>>(_AppConfig.WebApiHost + "api/Role/GetRoleMenusByRoleId?roleId=" + id);
            viewModel.RolePermissions = APIInvoker.Get<IEnumerable<Models.Role.RoleDataPermission>>(_AppConfig.WebApiHost + "api/DataPermission/GetRoleDataPermissions?roleId=" + id);

            return View(viewModel);
        }

        #region EditHandler
        [HttpPost]
        public IActionResult EditHandler(Models.Role.RoleViewModel viewModel)
        {
            if (viewModel != null) {
                string apiUrl = _AppConfig.WebApiHost + "api/Role/UpdateRole";
                var role = new
                {
                    Id = viewModel.Id,
                    RoleName = viewModel.RoleName.Trim(),
                    UpdateTime = DateTime.Now,
                    OrgId = _AppUser.OrgId
                };
                bool result = APIInvoker.Post<bool>(apiUrl, role);

                if (result) {
                    // 1.1ɾ���ɵĽ�ɫ�Ͳ˵��Ĺ�ϵ
                    string delRoleMenuApiUrl = _AppConfig.WebApiHost + "api/Role/DeleteRoleMenuByRoleId?roleId=" + viewModel.Id;
                    int delRMCount = APIInvoker.Get<int>(delRoleMenuApiUrl);

                    // 1.2��ȡ��ɫ�Ͷ����˵��Ĺ�ϵ
                    List<object> roleMenuRelations = new List<object>();
                    string menuIds = Request.Form["RoleMenu"];
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

                    // 1.3��ȡ��ɫ��һ���˵��Ĺ�ϵ
                    List<int> parentMenuIds = new List<int>();
                    List<Models.Role.Menu> menus = APIInvoker.Get<List<Models.Role.Menu>>(_AppConfig.WebApiHost + "api/MenuPage/GetAllMenus");

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

                    // 1.4�������ݿ�
                    if (roleMenuRelations != null && roleMenuRelations.Count > 0) {
                        string roleMenuInsertApiUrl = _AppConfig.WebApiHost + "api/Role/RoleMenuBatchInsert";
                        bool rmInsertUrl = APIInvoker.Post<bool>(roleMenuInsertApiUrl, roleMenuRelations);
                    }

                    // 2.1ɾ����ɫ������Ȩ�޵Ĺ�ϵ
                    string delRolePermissionApiUrl = _AppConfig.WebApiHost + "api/DataPermission/DeleteRoleDataPermissions?roleId=" + viewModel.Id;
                    int delRPCount = APIInvoker.Get<int>(delRolePermissionApiUrl);

                    // 2.2��ȡ��ɫ������Ȩ�޵Ĺ�ϵ
                    List<object> rolePermissions = new List<object>();
                    // 2.2.1��ȡ��Դ�ɼ���Χ
                    rolePermissions.Add(new {
                        RoleId = viewModel.Id,
                        PermissionCategoryId = 1,
                        PermissionId = Convert.ToInt32(Request.Form["ResourceVisible"]),
                        CreateTime = DateTime.Now
                    });
                    // 2.2.2��ȡ��Դ����Ȩ��
                    string permissionIds = Request.Form["ResourceHandle"];
                    if (!string.IsNullOrEmpty(permissionIds)) {
                        string[] permissionIdArr = permissionIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (permissionIdArr.Length > 0) {
                            foreach (var permissionId in permissionIdArr) {
                                rolePermissions.Add(new
                                {
                                    RoleId = viewModel.Id,
                                    PermissionCategoryId = 2,
                                    PermissionId = permissionId,
                                    CreateTime = DateTime.Now
                                });
                            }
                        }
                    }

                    // 2.3�������ݿ�
                    if(rolePermissions != null && rolePermissions.Count > 0) {
                        string rolePermissionInsertApiUrl = _AppConfig.WebApiHost + "api/DataPermission/RoleDataPermissionBatchInsert";
                        bool rpInsertResult = APIInvoker.Post<bool>(rolePermissionInsertApiUrl, rolePermissions);
                    }

                    TempData["result"] = true;
                }
                else {
                    TempData["result"] = false;
                }
                return RedirectToAction("List");
            }
            return View();
        } 
        #endregion
    }
}