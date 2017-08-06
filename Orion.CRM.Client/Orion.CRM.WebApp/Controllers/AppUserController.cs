using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orion.CRM.WebTools;
using Orion.CRM.WebApp.App_Data;
using Microsoft.Extensions.Caching.Memory;

namespace Orion.CRM.WebApp.Controllers
{
    /// <summary>
    /// 用户管理控制器
    /// </summary>
    public class AppUserController : BaseController
    {
        private IMemoryCache _memoryCache;
        public AppUserController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        #region 用户列表
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pi">此处的pi是页索引PageIndex</param>
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

            //分页参数
            ViewBag.PagerOption = pageOption;

            //数据
            return View(list);
        } 
        #endregion

        #region 创建新用户
        public IActionResult Create()
        {
            Models.AppUser.AppUserViewModel viewModel = new Models.AppUser.AppUserViewModel();

            viewModel.RoleList = AppDTO.GetRoleListFromDb(_AppUser.OrgId);
            viewModel.ProjectList = AppDTO.GetProjectsFromDb(_AppUser.OrgId);


            return View(viewModel);
        } 
        #endregion

        #region 创建新用户处理程序
        [HttpPost]
        public IActionResult CreateHandler(Models.AppUser.AppUserViewModel viewModel)
        {
            if (viewModel != null) {
                string apiUrl = _AppConfig.WebApiHost + "api/AppUser/InsertUser";
                var user = new
                {
                    OrgId = _AppUser.OrgId,
                    UserName = viewModel.UserName.Trim(),
                    Password = Md5Encrypt.Md5Bit32(viewModel.Password.Trim()),
                    RealName = viewModel.RealName.Trim(),
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
                    string userRoleApi = _AppConfig.WebApiHost + "api/AppUser/InsertUserRole";
                    var userRole = new
                    {
                        UserId = userId,
                        RoleId = viewModel.RoleId,
                        CreateTime = DateTime.Now
                    };
                    int userRoleId = APIInvoker.Post<int>(userRoleApi, userRole);

                    // 插入用户和项目之间的关系
                    string userProjectApi = _AppConfig.WebApiHost + "api/AppUser/InsertUserProject";
                    var userProject = new
                    {
                        UserId = userId,
                        ProjectId = viewModel.ProjectId,
                        CreateTime = DateTime.Now
                    };
                    int userProjectId = APIInvoker.Post<int>(userProjectApi, userProject);

                    // 插入用户和业务组之间的关系
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

        #region 编辑新用户
        public IActionResult Edit(int id)
        {
            string url = _AppConfig.WebApiHost + "api/AppUser/GetUserById?id=" + id;
            Models.AppUser.AppUserViewModel viewModel = APIInvoker.Get<Models.AppUser.AppUserViewModel>(url);

            viewModel.RoleList = AppDTO.GetRoleListFromDb(_AppUser.OrgId);
            viewModel.ProjectList = AppDTO.GetProjectsFromDb(_AppUser.OrgId);

            return View(viewModel);
        } 
        #endregion

        #region 编辑用户处理程序
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
                        Password = user.Password.Trim(),
                        RealName = viewModel.RealName.Trim(),
                        UpdateTime = DateTime.Now,
                        Mobile = viewModel.Mobile,
                        Email = viewModel.Email,
                        Wechat = viewModel.Wechat,
                        Enable = viewModel.Enable
                    };
                    bool result = APIInvoker.Post<bool>(apiUrl, updatingUser);
                    if (result) {
                        // 修改用户和角色之间的关系
                        string userRoleApi = _AppConfig.WebApiHost + "api/AppUser/UpdateUserRole";
                        var userRole = new
                        {
                            UserId = viewModel.Id,
                            RoleId = viewModel.RoleId
                        };

                        bool res1 = APIInvoker.Post<bool>(userRoleApi, userRole);

                        if (viewModel.ProjectId == null || viewModel.ProjectId == 0) {
                            // 删除用户和项目之间的关系
                            APIInvoker.Get<int>(_AppConfig.WebApiHost + "api/AppUser/DeleteUserProject?userId=" + user.Id);
                        }
                        else {
                            var query = APIInvoker.Get<Models.AppUser.UserProject>(_AppConfig.WebApiHost + "api/AppUser/GetUserProject?userId=" + user.Id);
                            if (query == null) {
                                // 插入用户和项目之间的关系
                                string userProjectApi = _AppConfig.WebApiHost + "api/AppUser/InsertUserProject";
                                var userProject = new
                                {
                                    UserId = user.Id,
                                    ProjectId = viewModel.ProjectId,
                                    CreateTime = DateTime.Now
                                };
                                int userProjectId = APIInvoker.Post<int>(userProjectApi, userProject);
                            }
                            else { 
                                // 修改用户和项目之间的关系
                                string userProjectApi = _AppConfig.WebApiHost + "api/AppUser/UpdateUserProject";
                                var userProject = new
                                {
                                    UserId = user.Id,
                                    ProjectId = viewModel.ProjectId
                                };
                                bool res2 = APIInvoker.Post<bool>(userProjectApi, userProject);
                            }
                        }

                        if(viewModel.GroupId == null || viewModel.GroupId == 0) {
                            // 删除用户和业务组之间的关系
                            APIInvoker.Get<int>(_AppConfig.WebApiHost + "api/AppUser/DeleteUserGroup?userId=" + user.Id);
                        }
                        else {
                            var query = APIInvoker.Get<Models.AppUser.UserGroup>(_AppConfig.WebApiHost + "api/AppUser/GetUserGroup?userId=" + user.Id);
                            if(query == null) {
                                // 插入用户和业务组之间的关系
                                string userGroupApi = _AppConfig.WebApiHost + "api/AppUser/InsertUserGroup";
                                var userGroup = new
                                {
                                    UserId = user.Id,
                                    GroupId = viewModel.GroupId,
                                    CreateTime = DateTime.Now
                                };
                                var userGroupId = APIInvoker.Post<int>(userGroupApi, userGroup);
                            }
                            else { 
                                // 修改用户和业务组之间的关系
                                string userGroupApi = _AppConfig.WebApiHost + "api/AppUser/UpdateUserGroup";
                                var userGroup = new
                                {
                                    UserId = user.Id,
                                    GroupId = viewModel.GroupId
                                };
                                bool res3 = APIInvoker.Post<bool>(userGroupApi, userGroup);
                            }
                        }
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

        public IActionResult ResetPassword(int id)
        {
            Models.AppUser.AppUserViewModel user = GetUserById(id);
            if (user != null) {
                Models.AppUser.ResetPasswordModel viewModel = new Models.AppUser.ResetPasswordModel();
                viewModel.UserId = id;
                viewModel.UserName = user.UserName;
                viewModel.Realname = user.RealName;

                return View(viewModel);
            }

            return RedirectToAction("List");
        }

        [HttpPost]
        public IActionResult ResetPasswordHandler(Models.AppUser.ResetPasswordModel viewModel)
        {
            if (viewModel != null) {
                viewModel.Password = Md5Encrypt.Md5Bit32(viewModel.Password);
                string apiUrl = _AppConfig.WebApiHost + "api/AppUser/UpdatePassword?userId=" + viewModel.UserId + "&password=" + viewModel.Password;
                bool result = APIInvoker.Get<bool>(apiUrl);
                if (result) {
                    // 更新token
                    string tokenContent = _AppUser.UserName + "," + viewModel.Password;
                    string token = DesEncrypt.Encrypt(tokenContent, _AppConfig.DesEncryptKey);
                    // 将token保存在服务器端缓存中(永不过期)
                    var cacheOptons = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(30));
                    _memoryCache.Set("token_" + viewModel.UserId, token, cacheOptons);
                }
                TempData["result"] = result;
            }
            return RedirectToAction("ResetPassword");
        }

        public IActionResult ResourceExport()
        {
            return View();
        }

        public IActionResult ResourceExportHandler()
        {
            return View();
        }

        // 通用Ajax方法
        #region 获取业务组下的成员
        /// <summary>
        /// 获取业务组下的成员
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
        #endregion

        #region 根据Id获取用户
        /// <summary>
        /// 根据Id获取用户
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
        #endregion

        #region 验证密码
        /// <summary>
        /// 验证密码
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
        #endregion
    }
}