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

            //当前登录用户
            ViewBag.CurrentUser = _AppUser.Id;

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
                int? old_projectId = 0, old_groupId = 0;

                if (user != null) {
                    old_projectId = user.ProjectId;
                    old_groupId = user.GroupId;
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
                                InsertUserProject(user.Id, Convert.ToInt32(viewModel.ProjectId));// 插入用户和项目之间的关系
                            }
                            else {
                                UpdateUserProject(user.Id, Convert.ToInt32(viewModel.ProjectId));// 修改用户和项目之间的关系
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
                                InsertUserGroup(user.Id, Convert.ToInt32(viewModel.GroupId));
                            }
                            else { 
                                // 修改用户和业务组之间的关系
                                UpdateUserGroup(user.Id, Convert.ToInt32(viewModel.GroupId));
                            }
                        }

                        // 先获取此用户所有的资源
                        List<int> resourceIdList = APIInvoker.Get<List<int>>(_AppConfig.WebApiHost + "api/Resource/GetResourcesByUserId?userId=" + viewModel.Id);
                        if(resourceIdList != null && resourceIdList.Count > 0) {
                            string resourceIds = string.Join(",", resourceIdList);
                            // 1.处理用户的资源和Project之间的关系
                            if(viewModel.ProjectId == null || viewModel.ProjectId <= 0) {
                                // 删除此用户的所有资源和此Project之间的关系
                                APIInvoker.Get<int>(_AppConfig.WebApiHost + "api/Resource/BatchDeleteResourceProject?resourceIds=" + resourceIds);
                            }
                            else if(viewModel.ProjectId != old_projectId) {
                                // 修改
                                APIInvoker.Get<int>(_AppConfig.WebApiHost + $"api/Resource/UpdateResourceProjectByResourceIds?resourceIds={resourceIds}&projectId={viewModel.ProjectId}");
                            }

                            // 2.处理用户的资源和Group之间的关系
                            if(viewModel.GroupId == null || viewModel.GroupId <= 0) {
                                // 删除此用户的所有资源和此Group之间的关系
                                APIInvoker.Get<int>(_AppConfig.WebApiHost + "api/Resource/BatchDeleteResourceGroup?resourceIds=" + resourceIds);
                            }
                            else if(viewModel.GroupId != old_groupId) {
                                // 修改
                                APIInvoker.Get<int>(_AppConfig.WebApiHost + $"api/Resource/UpdateResourceGroupByResourceIds?resourceIds={resourceIds}&groupId={viewModel.GroupId}");
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

        #region 重置用户密码
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
        #endregion

        #region 资源导出
        public IActionResult ResourceExport(int id)
        {
            Models.AppUser.ResourceExportModel viewModel = new Models.AppUser.ResourceExportModel();
            viewModel.UserId = id;

            var appUser = GetUserById(id);
            if (appUser != null) {
                viewModel.GroupList = AppDTO.GetGroupsFromDb(Convert.ToInt32(appUser.ProjectId));
                viewModel.UserName = appUser.UserName;
                viewModel.RealName = appUser.RealName;
                viewModel.ResourceCount = GetTalkingResourceCountByUserId(id);
            }

            return View(viewModel);
        } 
        #endregion

        #region 资源导出-处理程序
        [HttpPost]
        public IActionResult ResourceExportHandler(Models.AppUser.ResourceExportModel viewModel)
        {
            if (viewModel != null && viewModel.UserId > 0) {
                switch (viewModel.ExportDirection) {
                    case "public":
                        // 导入到公库
                        var apiPublic = _AppConfig.WebApiHost + "api/Resource/AssignUserResourcesToPublic?userId=" + viewModel.UserId;
                        APIInvoker.Get<object>(apiPublic);
                        break;
                    case "unassign":
                        // 导入到未分配
                        var apiUnassigned = _AppConfig.WebApiHost + "api/Resource/AssignUserResourcesToUnassigned?userId=" + viewModel.UserId;
                        APIInvoker.Get<object>(apiUnassigned);
                        break;
                    default:
                        int targetGroupId = int.Parse(viewModel.ExportDirection);
                        if (!string.IsNullOrEmpty(viewModel.ExportTarget)) {
                            // --------------------导入到某一用户名下--------------------
                            int targetUserId = int.Parse(viewModel.ExportTarget);

                            // ResourceGroup关系处理
                            string resourceGroupChangeApi = _AppConfig.WebApiHost + "api/Resource/ChangeResourceGroupOwner?sourceUserId=" + viewModel.UserId + "&targetGroupId=" + targetGroupId;
                            int count1 = APIInvoker.Get<int>(resourceGroupChangeApi);

                            // ResourceUser关系处理
                            string resourceUserChangeApi = _AppConfig.WebApiHost + "api/Resource/ChangeResourceUserOwner?sourceUserId=" + viewModel.UserId + "&targetUserId=" + targetUserId;
                            int count2 = APIInvoker.Get<int>(resourceUserChangeApi);

                            // 写入洽谈记录
                            List<int> resourceIds = APIInvoker.Get<List<int>>(_AppConfig.WebApiHost + "api/Resource/GetResourcesByUserId?userId=" + targetUserId);
                            var sourceUser = GetUserById(viewModel.UserId);//源用户
                            var targetUser = GetUserById(int.Parse(viewModel.ExportTarget));//目标用户
                            List<object> talkRecords = new List<object>();
                            DateTime now = DateTime.Now;
                            foreach(var resourceId in resourceIds) {
                                var talkRecord = new
                                {
                                    ResourceId = resourceId,
                                    TalkWay = 5,
                                    TalkResult = _AppUser.RealName + $"使用\"资源导出\"功能将{sourceUser.RealName}名下的资源导出给{targetUser.RealName}",
                                    UserId = _AppUser.Id,
                                    Type = 1,
                                    CreateTime = now
                                };
                                talkRecords.Add(talkRecord);
                            }
                            bool result = APIInvoker.Post<bool>(_AppConfig.WebApiHost + "api/TalkRecord/TalkRecordBatchInsert", talkRecords);
                        }
                        else {
                            // --------------------导入到业务组--------------------
                            // step1.获取该用户的所有资源Id
                            List<int> resourceIds = APIInvoker.Get<List<int>>(_AppConfig.WebApiHost + "api/Resource/GetResourcesByUserId?userId=" + viewModel.UserId);
                            if (resourceIds != null && resourceIds.Count > 0) {
                                string resourceIdStr = string.Join(",", resourceIds);
                                // step2.删除Resource和该用户的关系
                                string deleteUserApi = _AppConfig.WebApiHost + "api/Resource/DeleteResourceUserByUserId?userId=" + viewModel.UserId;
                                int count3 = APIInvoker.Get<int>(deleteUserApi);

                                // step3.删除Resource和该用户所在组的关系
                                string deleteGroupApi = _AppConfig.WebApiHost + "api/Resource/DeleteResourceGroupByResourceIds?resourceIds=" + resourceIdStr;
                                int count4 = APIInvoker.Get<int>(deleteGroupApi);

                                // step4.添加新的ResourceGroup关系
                                string resourceGroupBatchApi = _AppConfig.WebApiHost + "api/Resource/ResourceGroupBatchInsert";
                                List<object> resourceGroups = new List<object>();
                                if (resourceIds != null && resourceIds.Count > 0) {
                                    DateTime now = DateTime.Now;
                                    foreach (var resourceId in resourceIds) {
                                        resourceGroups.Add(new
                                        {
                                            ResourceId = resourceId,
                                            GroupId = targetGroupId,
                                            CreateTime = now
                                        });
                                    }
                                }
                                // step5.将上述关系批量插入数据库
                                if (resourceGroups != null && resourceGroups.Count > 0) {
                                    bool rgInsertResult = APIInvoker.Post<bool>(resourceGroupBatchApi, resourceGroups);
                                }
                                // step6.将这些资源的状态设置为未分配
                                string batchSetResourceStatusApi = _AppConfig.WebApiHost + "api/Resource/BatchSetResourceStatus?resourceIds=" + resourceIdStr + "&status=3";
                                APIInvoker.Get<int>(batchSetResourceStatusApi);
                            }
                        }
                        break;
                }
                TempData["result"] = true;
                return RedirectToAction("ResourceExport", "AppUser", new { id = viewModel.UserId });
            }
            return RedirectToAction("Index", "Error");
        } 
        #endregion

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

        #region 获取用户洽谈中的资源的总条数(注意洽谈中)
        // 获取用户洽谈中的资源的总条数(注意洽谈中)
        [HttpGet]
        public int GetTalkingResourceCountByUserId(int userId)
        {
            string apiUrl = _AppConfig.WebApiHost + "api/Resource/GetTalkingResourceCountByUserId?userId=" + userId;
            int count = APIInvoker.Get<int>(apiUrl);

            return count;
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

        #region 获取该用户下的资源个数
        private int GetResourceCountByUserId(int userId)
        {
            string apiUrl = _AppConfig.WebApiHost + "api/Resource/GetResourceCountByUserId?userId=" + userId;
            int count = APIInvoker.Get<int>(apiUrl);

            return count;
        }
        #endregion

        #region 插入资源和业务组的关系
        private int InsertResourceGroup(int resourceId, int groupId)
        {
            var resourceGroup = new
            {
                ResourceId = resourceId,
                GroupId = groupId,
                CreateTime = DateTime.Now
            };

            string apiUrl = _AppConfig.WebApiHost + "api/Resource/InsertResourceGroup";
            int identityId = APIInvoker.Post<int>(apiUrl, resourceGroup);

            return identityId;
        }
        #endregion

        #region DeleteUser
        public bool DeleteUser(int id)
        {
            string apiUrl = _AppConfig.WebApiHost + "api/AppUser/DeleteUser?userId=" + id;
            int count = APIInvoker.Get<int>(apiUrl);
            if (count > 0) {
                TempData["result"] = true;
                return true;
            }
            return false;
        }
        #endregion

        #region 判断该Email是否被其他用户使用
        // 判断该Email是否被其他用户使用，True表示已被其他用户使用，False表示没有被其他用户使用
        public bool CheckEmailExist(string email, int userId)
        {
            if (string.IsNullOrEmpty(email) || userId <= 0) return true;

            var appUser = AppDTO.GetUserByEmail(email);
            if (appUser != null) {
                if (appUser.Id != userId) {
                    return true;//已被其他用户使用
                }
            }

            return false;
        }
        #endregion

        #region 修改用户和项目之间的关系
        private bool UpdateUserProject(int userId, int projectId)
        {
            string userProjectApi = _AppConfig.WebApiHost + "api/AppUser/UpdateUserProject";
            var userProject = new
            {
                UserId = userId,
                ProjectId = projectId
            };
            bool res = APIInvoker.Post<bool>(userProjectApi, userProject);
            return res;
        }
        #endregion

        #region 插入用户和项目之间的关系
        private int InsertUserProject(int userId, int projectId)
        {
            string userProjectApi = _AppConfig.WebApiHost + "api/AppUser/InsertUserProject";
            var userProject = new
            {
                UserId = userId,
                ProjectId = projectId,
                CreateTime = DateTime.Now
            };
            int userProjectId = APIInvoker.Post<int>(userProjectApi, userProject);
            return userProjectId;
        }
        #endregion

        #region 修改用户和业务组之间的关系
        private bool UpdateUserGroup(int userId, int groupId)
        {
            string userGroupApi = _AppConfig.WebApiHost + "api/AppUser/UpdateUserGroup";
            var userGroup = new
            {
                UserId = userId,
                GroupId = groupId
            };
            bool res = APIInvoker.Post<bool>(userGroupApi, userGroup);
            return res;
        }
        #endregion

        #region 插入用户和业务组之间的关系
        private int InsertUserGroup(int userId, int groupId)
        {
            string userGroupApi = _AppConfig.WebApiHost + "api/AppUser/InsertUserGroup";
            var userGroup = new
            {
                UserId = userId,
                GroupId = groupId,
                CreateTime = DateTime.Now
            };
            var userGroupId = APIInvoker.Post<int>(userGroupApi, userGroup);
            return userGroupId;
        } 
        #endregion
    }
}