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
        /// <param name="pi">�˴���pi��ҳ����PageIndex</param>
        /// <returns></returns>
        public IActionResult List(int pi = 1)
        {
            string url = _AppConfig.WebApiHost + "api/AppUser/GetUsersByOrgId?pageIndex=" + pi + "&pageSize=" + _AppConfig.PageSize + "&orgId=" + _AppUser.OrgId;
            List<Models.AppUser.AppUserViewModel> list = APIInvoker.Get<List<Models.AppUser.AppUserViewModel>>(url);

            int totalCount = APIInvoker.Get<int>(_AppConfig.WebApiHost + "api/AppUser/GetUserCountByOrgId?orgId=" + _AppUser.OrgId);

            var pageOption = new PagerOption {
                PageIndex = pi,
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

            viewModel.RoleList = AppDTO.GetRoleListFromDb(_AppConfig.WebApiHost, _AppUser.OrgId);
            viewModel.ProjectList = AppDTO.GetProjectsFromDb(_AppConfig.WebApiHost, _AppUser.OrgId);


            return View(viewModel);
        }

        #region �����������
        [HttpPost]
        public IActionResult CreateHandler(Models.AppUser.AppUserViewModel viewModel)
        {
            if (viewModel != null) {
                string apiUrl = _AppConfig.WebApiHost + "api/AppUser/InsertUser";
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
                    // �����û��ͽ�ɫ֮��Ĺ�ϵ
                    string userRoleApi = _AppConfig.WebApiHost + "api/AppUser/InsertUserRole";
                    var userRole = new
                    {
                        UserId = userId,
                        RoleId = viewModel.RoleId,
                        CreateTime = DateTime.Now
                    };
                    int userRoleId = APIInvoker.Post<int>(userRoleApi, userRole);

                    // �����û�����Ŀ֮��Ĺ�ϵ
                    string userProjectApi = _AppConfig.WebApiHost + "api/AppUser/InsertUserProject";
                    var userProject = new
                    {
                        UserId = userId,
                        ProjectId = viewModel.ProjectId,
                        CreateTime = DateTime.Now
                    };
                    int userProjectId = APIInvoker.Post<int>(userProjectApi, userProject);

                    // �����û���ҵ����֮��Ĺ�ϵ
                    string userGroupApi = _AppConfig.WebApiHost + "api/AppUser/InsertUserGroup";
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
        #endregion

        public IActionResult Edit(int id)
        {
            string url = _AppConfig.WebApiHost + "api/AppUser/GetUserById?id=" + id;
            Models.AppUser.AppUserViewModel viewModel = APIInvoker.Get<Models.AppUser.AppUserViewModel>(url);

            viewModel.RoleList = AppDTO.GetRoleListFromDb(_AppConfig.WebApiHost, _AppUser.OrgId);
            viewModel.ProjectList = AppDTO.GetProjectsFromDb(_AppConfig.WebApiHost, _AppUser.OrgId);

            return View(viewModel);
        }

        #region �޸Ĵ������
        [HttpPost]
        public IActionResult EditHandler(Models.AppUser.AppUserViewModel viewModel)
        {
            if (viewModel != null) {
                string url = _AppConfig.WebApiHost + "api/AppUser/GetUserById?id=" + viewModel.Id;
                Models.AppUser.AppUserViewModel user = APIInvoker.Get<Models.AppUser.AppUserViewModel>(url);

                if (user != null) {
                    string apiUrl = _AppConfig.WebApiHost + "api/AppUser/UpdateUser";
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
                        string userRoleApi = _AppConfig.WebApiHost + "api/AppUser/UpdateUserRole";
                        var userRole = new
                        {
                            UserId = viewModel.Id,
                            RoleId = viewModel.RoleId
                        };

                        bool res1 = APIInvoker.Post<bool>(userRoleApi, userRole);

                        // �޸��û�����Ŀ֮��Ĺ�ϵ
                        string userProjectApi = _AppConfig.WebApiHost + "api/AppUser/UpdateUserProject";
                        var userProject = new
                        {
                            UserId = user.Id,
                            ProjectId = viewModel.ProjectId
                        };
                        bool res2 = APIInvoker.Post<bool>(userProjectApi, userProject);

                        // �޸��û���ҵ����֮��Ĺ�ϵ
                        string userGroupApi = _AppConfig.WebApiHost + "api/AppUser/UpdateUserGroup";
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
        #endregion

        // ͨ��Ajax����
        /// <summary>
        /// ��ȡҵ�����µĳ�Ա
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpGet]
        public List<Models.AppUser.AppUserViewModel> GetUsersByGroupId(int groupId)
        {
            string apiUrl = _AppConfig.WebApiHost + "api/AppUser/GetAllUsersByGroupId?groupId=" + groupId;
            List<Models.AppUser.AppUserViewModel> users = APIInvoker.Get<List<Models.AppUser.AppUserViewModel>>(apiUrl);
            return users;
        }

        /// <summary>
        /// ����Id��ȡ�û�
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public Models.AppUser.AppUserViewModel GetUserById(int userId)
        {
            string apiUrl = _AppConfig.WebApiHost + "api/AppUser/GetUserById?id=" + userId;
            Models.AppUser.AppUserViewModel appUser = APIInvoker.Get<Models.AppUser.AppUserViewModel>(apiUrl);
            return appUser;
        }

        /// <summary>
        /// ��֤����
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        public bool ValidatePassword(int userId, string password)
        {
            string apiUrl = _AppConfig.WebApiHost + "api/AppUser/GetUserById?id=" + userId;
            Models.AppUser.AppUserViewModel appUser = APIInvoker.Get<Models.AppUser.AppUserViewModel>(apiUrl);
            if (appUser != null && appUser.Password == Md5Encrypt.Md5Bit32(password)) {
                return true;
            }
            return false;
        }
    }
}