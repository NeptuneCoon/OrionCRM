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
    /// �û����������
    /// </summary>
    public class AppUserController : BaseController
    {
        /// <summary>
        /// �û��б�
        /// </summary>
        /// <param name="id">�˴���id��ҳ����PageIndex</param>
        /// <returns></returns>
        public IActionResult List(int id = 1)
        {
            string url = _AppConfig.WebAPIHost + "api/AppUser/GetUsersByOrgId?pageIndex=" + id + "&pageSize=" + _AppConfig.PageSize + "&orgId=" + _AppUser.OrgId;
            List<Models.AppUser.AppUserViewModel> list = APIInvoker.Get<List<Models.AppUser.AppUserViewModel>>(url);

            int totalCount = APIInvoker.Get<int>(_AppConfig.WebAPIHost + "api/AppUser/GetUserCountByOrgId?orgId=" + _AppUser.OrgId);

            var pageOption = new PagerOption {
                PageIndex = id,
                PageSize = _AppConfig.PageSize,
                TotalCount = totalCount,
                RouteUrl = "/AppUser/List"
            };

            //��ҳ����
            ViewBag.PagerOption = pageOption;

            //����
            return View(list);
        }

        public IActionResult Create()
        {
            Models.AppUser.AppUserViewModel viewModel = new Models.AppUser.AppUserViewModel();

            string roleApiUrl = _AppConfig.WebAPIHost + "api/Role/GetRolesByOrgId?pageIndex=1&pageSize=10000&orgId=" + _AppUser.OrgId;
            viewModel.RoleList = APIInvoker.Get<List<Models.Role.Role>>(roleApiUrl);

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CreateHandler(Models.AppUser.AppUserViewModel viewModel)
        {
            if (viewModel != null) {
                string apiUrl = _AppConfig.WebAPIHost + "api/AppUser/InsertUser";
                var user = new
                {
                    OrgId = _AppUser.OrgId,
                    UserName = viewModel.UserName,
                    Password = Md5Encrypt.Md5Bit32(viewModel.Password),
                    RealName = viewModel.RealName,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    Mobile = viewModel.Mobile,
                    Email = viewModel.Email,
                    Wechat = viewModel.Wechat,
                    Enable = viewModel.Enable
                };

                int primaryId = APIInvoker.Post<int>(apiUrl, user);
                if (primaryId > 0) {
                    // �����û��ͽ�ɫ֮��Ĺ�ϵ
                    string userRoleApi = _AppConfig.WebAPIHost + "api/AppUser/InsertUserRole";
                    var userRole = new
                    {
                        UserId = primaryId,
                        RoleId = viewModel.RoleId,
                        CreateTime = DateTime.Now
                    };
                    int userRoleId = APIInvoker.Post<int>(userRoleApi, userRole);
                }

                TempData["result"] = primaryId > 0;
                return RedirectToAction("List");
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            string url = _AppConfig.WebAPIHost + "api/AppUser/GetUserById?id=" + id;
            Models.AppUser.AppUserViewModel viewModel = APIInvoker.Get<Models.AppUser.AppUserViewModel>(url);

            string roleApiUrl = _AppConfig.WebAPIHost + "api/Role/GetRolesByOrgId?pageIndex=1&pageSize=10000&orgId=" + _AppUser.OrgId;
            viewModel.RoleList = APIInvoker.Get<List<Models.Role.Role>>(roleApiUrl);

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult EditHandler(Models.AppUser.AppUserViewModel viewModel)
        {
            if (viewModel != null) {
                string url = _AppConfig.WebAPIHost + "api/AppUser/GetUserById?id=" + viewModel.Id;
                Models.AppUser.AppUserViewModel user = APIInvoker.Get<Models.AppUser.AppUserViewModel>(url);

                if (user != null) { 
                    string apiUrl = _AppConfig.WebAPIHost + "api/AppUser/UpdateUser";
                    var updatingUser = new
                    {
                        Id = user.Id,
                        OrgId = user.OrgId,
                        UserName = user.UserName,
                        Password = user.Password,
                        RealName = viewModel.RealName,
                        UpdateTime = DateTime.Now,
                        Mobile = viewModel.Mobile,
                        Email = viewModel.Email,
                        Wechat = viewModel.Wechat,
                        Enable = viewModel.Enable
                    };
                    bool result = APIInvoker.Post<bool>(apiUrl, updatingUser);
                    if (result) {
                        // �޸��û��ͽ�ɫ֮��Ĺ�ϵ
                        string userRoleApi = _AppConfig.WebAPIHost + "api/AppUser/UpdateUserRole";
                        var userRole = new
                        {
                            UserId = viewModel.Id,
                            RoleId = viewModel.RoleId
                        };

                        bool _res = APIInvoker.Post<bool>(userRoleApi, userRole);
                    
                    }

                    TempData["result"] = result;
                }
                else {
                    TempData["result"] = false;
                }
                return RedirectToAction("List");
            }
            return View();
        }
    }
}