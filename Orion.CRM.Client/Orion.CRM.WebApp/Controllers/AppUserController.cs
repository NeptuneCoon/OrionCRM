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
    /// 用户管理控制器
    /// </summary>
    public class AppUserController : BaseController
    {
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="id">此处的id是页索引PageIndex</param>
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

            //分页参数
            ViewBag.PagerOption = pageOption;

            //数据
            return View(list);
        }

        public IActionResult Create()
        {
            Models.AppUser.AppUserViewModel viewModel = new Models.AppUser.AppUserViewModel();

            viewModel.RoleList = AppDTO.GetRoleListFromDb(_AppConfig.WebAPIHost, _AppUser.OrgId);
            viewModel.ProjectList = AppDTO.GetProjectsFromDb(_AppConfig.WebAPIHost, _AppUser.OrgId);


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

                int userId = APIInvoker.Post<int>(apiUrl, user);
                if (userId > 0) {
                    // 插入用户和角色之间的关系
                    string userRoleApi = _AppConfig.WebAPIHost + "api/AppUser/InsertUserRole";
                    var userRole = new
                    {
                        UserId = userId,
                        RoleId = viewModel.RoleId,
                        CreateTime = DateTime.Now
                    };
                    int userRoleId = APIInvoker.Post<int>(userRoleApi, userRole);

                    // 插入用户和项目之间的关系
                    string userProjectApi = _AppConfig.WebAPIHost + "api/AppUser/InsertUserProject";
                    var userProject = new
                    {
                        UserId = userId,
                        ProjectId = viewModel.ProjectId,
                        CreateTime = DateTime.Now
                    };
                    int userProjectId = APIInvoker.Post<int>(userProjectApi, userProject);

                    // 插入用户和业务组之间的关系
                    string userGroupApi = _AppConfig.WebAPIHost + "api/AppUser/InsertUserGroup";
                    var userGroup = new
                    {
                        UserId = userId,
                        GroupId = viewModel.GroupId,
                        CreateTime = DateTime.Now
                    };
                    var userGroupId = APIInvoker.Post<int>(userGroupApi, userGroup);
                }

                TempData["result"] = userId > 0;
                return RedirectToAction("List");
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            string url = _AppConfig.WebAPIHost + "api/AppUser/GetUserById?id=" + id;
            Models.AppUser.AppUserViewModel viewModel = APIInvoker.Get<Models.AppUser.AppUserViewModel>(url);

            viewModel.RoleList = AppDTO.GetRoleListFromDb(_AppConfig.WebAPIHost, _AppUser.OrgId);
            viewModel.ProjectList = AppDTO.GetProjectsFromDb(_AppConfig.WebAPIHost, _AppUser.OrgId);

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
                        // 修改用户和角色之间的关系
                        string userRoleApi = _AppConfig.WebAPIHost + "api/AppUser/UpdateUserRole";
                        var userRole = new
                        {
                            UserId = viewModel.Id,
                            RoleId = viewModel.RoleId
                        };

                        bool res1 = APIInvoker.Post<bool>(userRoleApi, userRole);

                        // 修改用户和项目之间的关系
                        string userProjectApi = _AppConfig.WebAPIHost + "api/AppUser/UpdateUserProject";
                        var userProject = new
                        {
                            UserId = user.Id,
                            ProjectId = viewModel.ProjectId
                        };
                        bool res2 = APIInvoker.Post<bool>(userProjectApi, userProject);

                        // 修改用户和业务组之间的关系
                        string userGroupApi = _AppConfig.WebAPIHost + "api/AppUser/UpdateUserGroup";
                        var userGroup = new
                        {
                            UserId = user.Id,
                            GroupId = viewModel.GroupId
                        };
                        bool res3 = APIInvoker.Post<bool>(userGroupApi, userGroup);
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

        [HttpGet]
        public List<Models.Group.Group> GetGroupsByProjectId(int projectId)
        {
            return AppDTO.GetGroupsFromDb(_AppConfig.WebAPIHost, projectId);
        }
    }
}