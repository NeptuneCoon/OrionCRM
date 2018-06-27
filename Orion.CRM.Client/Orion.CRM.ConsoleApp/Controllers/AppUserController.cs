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
    public class AppUserController : Controller
    {
        private readonly AppConfig _appConfig;
        public AppUserController(IOptions<AppConfig> optionsAccessor)
        {
            _appConfig = optionsAccessor.Value;
        }

        /// <summary>
        /// 分页展示数据
        /// </summary>
        /// <param name="id">此处的id是页索引PageIndex</param>
        /// <returns></returns>
        public IActionResult UserList(int id = 1)
        {
            ViewBag.OperateResult = Request.Query["operateResult"].ToString();

            string url = _appConfig.WebAPIHost + "api/AppUser/GetUsers?pageIndex=" + id + "&pageSize=" + _appConfig.PageSize;
            List<Models.User.AppUserViewModel> list = APIInvoker.Get<List<Models.User.AppUserViewModel>>(url);

            int totalCount = APIInvoker.Get<int>(_appConfig.WebAPIHost + "api/AppUser/GetUserCount");

            var pageOption = new PagerOption {
                PageIndex = id,
                PageSize = _appConfig.PageSize,
                TotalCount = totalCount,
                RouteUrl = "/AppUser/UserList"
            };

            //分页参数
            ViewBag.PagerOption = pageOption;

            //数据
            return View(list);
        }

        public IActionResult Create()
        {
            Models.User.AppUserViewModel viewModel = new Models.User.AppUserViewModel();

            string orgApiUrl = _appConfig.WebAPIHost + "api/Organization/GetAllOrganizations";
            //string roleApiUrl = _appConfig.WebAPIHost + "api/Role/GetRolesByOrgId?pageIndex=1&pageSize=2000&orgId=";
            viewModel.OrgList = APIInvoker.Get<IEnumerable<Models.Organization.OrganizationViewModel>>(orgApiUrl);
            //viewModel.RoleList = APIInvoker.Get <IEnumerable<Models.Role.RoleViewModel>>(roleApiUrl);

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CreateHandler(Models.User.AppUserViewModel viewModel)
        {
            if (viewModel != null) {
                string apiUrl = _appConfig.WebAPIHost + "api/AppUser/InsertUser";
                var user = new {
                    OrgId = viewModel.OrgId,
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
                    return RedirectToAction("UserList", new { operateResult = "success" });
                }
                else {
                    return RedirectToAction("UserList", new { operateResult = "fail" });
                }
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            Models.User.AppUserViewModel viewModel = new Models.User.AppUserViewModel();

            string url = _appConfig.WebAPIHost + "api/AppUser/GetUserById?id=" + id;
            viewModel = APIInvoker.Get<Models.User.AppUserViewModel>(url);

            string orgApiUrl = _appConfig.WebAPIHost + "api/Organization/GetAllOrganizations";
            //string roleApiUrl = _appConfig.WebAPIHost + "api/Role/GetRolesByOrgId?pageIndex=1&pageSize=2000";
            viewModel.OrgList = APIInvoker.Get<IEnumerable<Models.Organization.OrganizationViewModel>>(orgApiUrl);
            //viewModel.RoleList = APIInvoker.Get<IEnumerable<Models.Role.RoleViewModel>>(roleApiUrl);

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult EditHandler(Models.User.AppUserViewModel viewModel)
        {
            if (viewModel != null) {
                string apiUrl = _appConfig.WebAPIHost + "api/AppUser/UpdateUser";
                var user = new {
                    Id = viewModel.Id,
                    OrgId = viewModel.OrgId,
                    UserName = viewModel.UserName,
                    Password = Md5Encrypt.Md5Bit32(viewModel.Password),
                    RealName = viewModel.RealName,
                    UpdateTime = DateTime.Now,
                    Mobile = viewModel.Mobile,
                    Email = viewModel.Email,
                    Wechat = viewModel.Wechat,
                    Enable = viewModel.Enable
                };
                bool result = APIInvoker.Post<bool>(apiUrl, user);

                if (result) {
                    return RedirectToAction("UserList", new { operateResult = "success" });
                }
                else {
                    return RedirectToAction("UserList", new { operateResult = "fail" });
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