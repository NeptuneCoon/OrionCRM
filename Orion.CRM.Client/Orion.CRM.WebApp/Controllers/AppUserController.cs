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
    /// �û����������
    /// </summary>
    public class AppUserController : BaseController
    {
        private IMemoryCache _memoryCache;
        public AppUserController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        #region �û��б�
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

            //��ǰ��¼�û�
            ViewBag.CurrentUser = _AppUser.Id;

            //����
            return View(list);
        } 
        #endregion

        #region �������û�
        public IActionResult Create()
        {
            Models.AppUser.AppUserViewModel viewModel = new Models.AppUser.AppUserViewModel();

            viewModel.RoleList = AppDTO.GetRoleListFromDb(_AppUser.OrgId);
            viewModel.ProjectList = AppDTO.GetProjectsFromDb(_AppUser.OrgId);


            return View(viewModel);
        } 
        #endregion

        #region �������û��������
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

        #region �༭���û�
        public IActionResult Edit(int id)
        {
            string url = _AppConfig.WebApiHost + "api/AppUser/GetUserById?id=" + id;
            Models.AppUser.AppUserViewModel viewModel = APIInvoker.Get<Models.AppUser.AppUserViewModel>(url);

            viewModel.RoleList = AppDTO.GetRoleListFromDb(_AppUser.OrgId);
            viewModel.ProjectList = AppDTO.GetProjectsFromDb(_AppUser.OrgId);

            return View(viewModel);
        } 
        #endregion

        #region �༭�û��������
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
                        // �޸��û��ͽ�ɫ֮��Ĺ�ϵ
                        string userRoleApi = _AppConfig.WebApiHost + "api/AppUser/UpdateUserRole";
                        var userRole = new
                        {
                            UserId = viewModel.Id,
                            RoleId = viewModel.RoleId
                        };

                        bool res1 = APIInvoker.Post<bool>(userRoleApi, userRole);

                        if (viewModel.ProjectId == null || viewModel.ProjectId == 0) {
                            // ɾ���û�����Ŀ֮��Ĺ�ϵ
                            APIInvoker.Get<int>(_AppConfig.WebApiHost + "api/AppUser/DeleteUserProject?userId=" + user.Id);
                        }
                        else {
                            var query = APIInvoker.Get<Models.AppUser.UserProject>(_AppConfig.WebApiHost + "api/AppUser/GetUserProject?userId=" + user.Id);
                            if (query == null) {
                                InsertUserProject(user.Id, Convert.ToInt32(viewModel.ProjectId));// �����û�����Ŀ֮��Ĺ�ϵ
                            }
                            else {
                                UpdateUserProject(user.Id, Convert.ToInt32(viewModel.ProjectId));// �޸��û�����Ŀ֮��Ĺ�ϵ
                            }
                        }

                        if(viewModel.GroupId == null || viewModel.GroupId == 0) {
                            // ɾ���û���ҵ����֮��Ĺ�ϵ
                            APIInvoker.Get<int>(_AppConfig.WebApiHost + "api/AppUser/DeleteUserGroup?userId=" + user.Id);
                        }
                        else {
                            var query = APIInvoker.Get<Models.AppUser.UserGroup>(_AppConfig.WebApiHost + "api/AppUser/GetUserGroup?userId=" + user.Id);
                            if(query == null) {
                                // �����û���ҵ����֮��Ĺ�ϵ
                                InsertUserGroup(user.Id, Convert.ToInt32(viewModel.GroupId));
                            }
                            else { 
                                // �޸��û���ҵ����֮��Ĺ�ϵ
                                UpdateUserGroup(user.Id, Convert.ToInt32(viewModel.GroupId));
                            }
                        }

                        // �Ȼ�ȡ���û����е���Դ
                        List<int> resourceIdList = APIInvoker.Get<List<int>>(_AppConfig.WebApiHost + "api/Resource/GetResourcesByUserId?userId=" + viewModel.Id);
                        if(resourceIdList != null && resourceIdList.Count > 0) {
                            string resourceIds = string.Join(",", resourceIdList);
                            // 1.�����û�����Դ��Project֮��Ĺ�ϵ
                            if(viewModel.ProjectId == null || viewModel.ProjectId <= 0) {
                                // ɾ�����û���������Դ�ʹ�Project֮��Ĺ�ϵ
                                APIInvoker.Get<int>(_AppConfig.WebApiHost + "api/Resource/BatchDeleteResourceProject?resourceIds=" + resourceIds);
                            }
                            else if(viewModel.ProjectId != old_projectId) {
                                // �޸�
                                APIInvoker.Get<int>(_AppConfig.WebApiHost + $"api/Resource/UpdateResourceProjectByResourceIds?resourceIds={resourceIds}&projectId={viewModel.ProjectId}");
                            }

                            // 2.�����û�����Դ��Group֮��Ĺ�ϵ
                            if(viewModel.GroupId == null || viewModel.GroupId <= 0) {
                                // ɾ�����û���������Դ�ʹ�Group֮��Ĺ�ϵ
                                APIInvoker.Get<int>(_AppConfig.WebApiHost + "api/Resource/BatchDeleteResourceGroup?resourceIds=" + resourceIds);
                            }
                            else if(viewModel.GroupId != old_groupId) {
                                // �޸�
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

        #region �����û�����
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
                    // ����token
                    string tokenContent = _AppUser.UserName + "," + viewModel.Password;
                    string token = DesEncrypt.Encrypt(tokenContent, _AppConfig.DesEncryptKey);
                    // ��token�����ڷ������˻�����(��������)
                    var cacheOptons = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(30));
                    _memoryCache.Set("token_" + viewModel.UserId, token, cacheOptons);
                }
                TempData["result"] = result;
            }
            return RedirectToAction("ResetPassword");
        }
        #endregion

        #region ��Դ����
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

        #region ��Դ����-�������
        [HttpPost]
        public IActionResult ResourceExportHandler(Models.AppUser.ResourceExportModel viewModel)
        {
            if (viewModel != null && viewModel.UserId > 0) {
                switch (viewModel.ExportDirection) {
                    case "public":
                        // ���뵽����
                        var apiPublic = _AppConfig.WebApiHost + "api/Resource/AssignUserResourcesToPublic?userId=" + viewModel.UserId;
                        APIInvoker.Get<object>(apiPublic);
                        break;
                    case "unassign":
                        // ���뵽δ����
                        var apiUnassigned = _AppConfig.WebApiHost + "api/Resource/AssignUserResourcesToUnassigned?userId=" + viewModel.UserId;
                        APIInvoker.Get<object>(apiUnassigned);
                        break;
                    default:
                        int targetGroupId = int.Parse(viewModel.ExportDirection);
                        if (!string.IsNullOrEmpty(viewModel.ExportTarget)) {
                            // --------------------���뵽ĳһ�û�����--------------------
                            int targetUserId = int.Parse(viewModel.ExportTarget);

                            // ResourceGroup��ϵ����
                            string resourceGroupChangeApi = _AppConfig.WebApiHost + "api/Resource/ChangeResourceGroupOwner?sourceUserId=" + viewModel.UserId + "&targetGroupId=" + targetGroupId;
                            int count1 = APIInvoker.Get<int>(resourceGroupChangeApi);

                            // ResourceUser��ϵ����
                            string resourceUserChangeApi = _AppConfig.WebApiHost + "api/Resource/ChangeResourceUserOwner?sourceUserId=" + viewModel.UserId + "&targetUserId=" + targetUserId;
                            int count2 = APIInvoker.Get<int>(resourceUserChangeApi);

                            // д��Ǣ̸��¼
                            List<int> resourceIds = APIInvoker.Get<List<int>>(_AppConfig.WebApiHost + "api/Resource/GetResourcesByUserId?userId=" + targetUserId);
                            var sourceUser = GetUserById(viewModel.UserId);//Դ�û�
                            var targetUser = GetUserById(int.Parse(viewModel.ExportTarget));//Ŀ���û�
                            List<object> talkRecords = new List<object>();
                            DateTime now = DateTime.Now;
                            foreach(var resourceId in resourceIds) {
                                var talkRecord = new
                                {
                                    ResourceId = resourceId,
                                    TalkWay = 5,
                                    TalkResult = _AppUser.RealName + $"ʹ��\"��Դ����\"���ܽ�{sourceUser.RealName}���µ���Դ������{targetUser.RealName}",
                                    UserId = _AppUser.Id,
                                    Type = 1,
                                    CreateTime = now
                                };
                                talkRecords.Add(talkRecord);
                            }
                            bool result = APIInvoker.Post<bool>(_AppConfig.WebApiHost + "api/TalkRecord/TalkRecordBatchInsert", talkRecords);
                        }
                        else {
                            // --------------------���뵽ҵ����--------------------
                            // step1.��ȡ���û���������ԴId
                            List<int> resourceIds = APIInvoker.Get<List<int>>(_AppConfig.WebApiHost + "api/Resource/GetResourcesByUserId?userId=" + viewModel.UserId);
                            if (resourceIds != null && resourceIds.Count > 0) {
                                string resourceIdStr = string.Join(",", resourceIds);
                                // step2.ɾ��Resource�͸��û��Ĺ�ϵ
                                string deleteUserApi = _AppConfig.WebApiHost + "api/Resource/DeleteResourceUserByUserId?userId=" + viewModel.UserId;
                                int count3 = APIInvoker.Get<int>(deleteUserApi);

                                // step3.ɾ��Resource�͸��û�������Ĺ�ϵ
                                string deleteGroupApi = _AppConfig.WebApiHost + "api/Resource/DeleteResourceGroupByResourceIds?resourceIds=" + resourceIdStr;
                                int count4 = APIInvoker.Get<int>(deleteGroupApi);

                                // step4.����µ�ResourceGroup��ϵ
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
                                // step5.��������ϵ�����������ݿ�
                                if (resourceGroups != null && resourceGroups.Count > 0) {
                                    bool rgInsertResult = APIInvoker.Post<bool>(resourceGroupBatchApi, resourceGroups);
                                }
                                // step6.����Щ��Դ��״̬����Ϊδ����
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

        // ͨ��Ajax����
        #region ��ȡҵ�����µĳ�Ա
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
        #endregion

        #region ����Id��ȡ�û�
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
        #endregion

        #region ��ȡ�û�Ǣ̸�е���Դ��������(ע��Ǣ̸��)
        // ��ȡ�û�Ǣ̸�е���Դ��������(ע��Ǣ̸��)
        [HttpGet]
        public int GetTalkingResourceCountByUserId(int userId)
        {
            string apiUrl = _AppConfig.WebApiHost + "api/Resource/GetTalkingResourceCountByUserId?userId=" + userId;
            int count = APIInvoker.Get<int>(apiUrl);

            return count;
        } 
        #endregion

        #region ��֤����
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
        #endregion

        #region ��ȡ���û��µ���Դ����
        private int GetResourceCountByUserId(int userId)
        {
            string apiUrl = _AppConfig.WebApiHost + "api/Resource/GetResourceCountByUserId?userId=" + userId;
            int count = APIInvoker.Get<int>(apiUrl);

            return count;
        }
        #endregion

        #region ������Դ��ҵ����Ĺ�ϵ
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

        #region �жϸ�Email�Ƿ������û�ʹ��
        // �жϸ�Email�Ƿ������û�ʹ�ã�True��ʾ�ѱ������û�ʹ�ã�False��ʾû�б������û�ʹ��
        public bool CheckEmailExist(string email, int userId)
        {
            if (string.IsNullOrEmpty(email) || userId <= 0) return true;

            var appUser = AppDTO.GetUserByEmail(email);
            if (appUser != null) {
                if (appUser.Id != userId) {
                    return true;//�ѱ������û�ʹ��
                }
            }

            return false;
        }
        #endregion

        #region �޸��û�����Ŀ֮��Ĺ�ϵ
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

        #region �����û�����Ŀ֮��Ĺ�ϵ
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

        #region �޸��û���ҵ����֮��Ĺ�ϵ
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

        #region �����û���ҵ����֮��Ĺ�ϵ
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