using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orion.CRM.WebApp.App_Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using Orion.CRM.WebTools;
using Microsoft.Extensions.Options;
using Orion.CRM.WebApp.Models.Resource;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;

namespace Orion.CRM.WebApp.Controllers
{
    /// <summary>
    /// ��Դ���������
    /// </summary>
    public class ResourceController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnv;
        public ResourceController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnv = hostingEnvironment;
        }

        const string SessionKeyName = "_Name";
        const string SessionKeyYearsMember = "_YearsMember";
        const string SessionKeyDate = "_Date";

        // ��Դ�б�
        public IActionResult List(ResourceSearchParams param)
        {
            Models.Resource.ResourceListViewModel viewModel = new ResourceListViewModel();
            viewModel.Params = param;

            // �������ʼ��
            viewModel.StatusList = AppDTO.GetStatusFromJson();
            viewModel.InclinationList = AppDTO.GetInclinationsFromJson();
            viewModel.SourceList = AppDTO.GetSourcesFromDb(_AppUser.OrgId);
            viewModel.TalkCountList = AppDTO.GetTalkCountFromJson();

            // Ȩ�޿���
            var roleDataPermissions = this.GetRoleDataPermissions(_AppUser.RoleId);

            viewModel.RoleResourceVisible = GetRoleResourceVisible(roleDataPermissions);
            viewModel.CanSearch = roleDataPermissions?.FirstOrDefault(x => x.PermissionId == 5) != null;
            viewModel.CanAssign = roleDataPermissions?.FirstOrDefault(x => x.PermissionId == 6) != null;
            viewModel.CanBatchAssign = roleDataPermissions?.FirstOrDefault(x => x.PermissionId == 7) != null;
            viewModel.CanDelete = roleDataPermissions?.FirstOrDefault(x => x.PermissionId == 10) != null;
            viewModel.RoleResourcePhoneVisible = GetRoleResourcePhoneVisible(roleDataPermissions);
            viewModel.ProjectId = _AppUser.ProjectId;

            if(param.pid != null && param.pid > 0) {
                viewModel.GroupList = AppDTO.GetGroupsByProjectId((int)param.pid);
            }
            if(param.gid != null && param.gid > 0) {
                viewModel.SalerList = AppDTO.GetUsersByGroupId((int)param.gid);
            }

            if (param.pi <= 0) param.pi = 1;
            param.ps = _AppConfig.PageSize;

            // ��ҳ����
            var pageOption = new PagerOption {
                PageIndex = param.pi,
                PageSize = param.ps,
                TotalCount = 0,
                RouteUrl = "/Resource/List",
                QueryString = Request.QueryString.Value
            };

            string groupApiUrl = _AppConfig.WebApiHost + "/api/Group/GetGroupsByProjectId?projectId=";
            string groupMemberApiUrl = _AppConfig.WebApiHost + "/api/AppUser/GetAllUsersByGroupId?groupId=";

            switch (viewModel.RoleResourceVisible) {
                case 4:
                    // ��Դ�ɼ���Χ����˾��Դ�������[��Ŀ�б�ҵ�����б�ҵ��Ա�б�]
                    viewModel.ProjectList = AppDTO.GetProjectsFromDb(_AppUser.OrgId);
                    break;
                case 3:
                    // ��Դ�ɼ���Χ������Ŀ��Դ�������[ҵ�����б�ҵ��Ա�б�]
                    viewModel.GroupList = APIInvoker.Get<List<Models.Group.Group>>(groupApiUrl + _AppUser.ProjectId);
                    param.pid = _AppUser.ProjectId;
                    break;
                case 2:
                    // ��Դ�ɼ���Χ��������Դ�������[ҵ��Ա�б�]
                    param.pid = _AppUser.ProjectId;
                    param.gid = _AppUser.GroupId;
                    viewModel.SalerList = APIInvoker.Get<List<Models.AppUser.AppUserViewModel>>(groupMemberApiUrl + param.gid);
                    break;
                case 1:
                    // ��Դ�ɼ���Χ��������Դ
                    param.pid = _AppUser.ProjectId;
                    param.gid = _AppUser.GroupId;
                    param.uid = _AppUser.Id;
                    break;
            }

            ViewBag.PagerOption = pageOption;

            // ��ѯ��������
            int totalCount = APIInvoker.Post<int>(_AppConfig.WebApiHost + "/api/Resource/GetResourceCountByCondition", param);
            pageOption.TotalCount = totalCount;

            string searchUrl = _AppConfig.WebApiHost + "/api/Resource/GetResourcesByCondition";
            List<Models.Resource.Resource> resources = APIInvoker.Post<List<Models.Resource.Resource>>(searchUrl, param);
            ResourceDataFormat(resources, viewModel.StatusList, viewModel.SourceList, viewModel.InclinationList, viewModel.RoleResourcePhoneVisible);
            viewModel.Resources = resources;

            // ��ȡ�û��Զ���ı�ǩ
            viewModel.Tags = GetTagList(_AppUser.Id);

            // ̸�����б�
            viewModel.TalkMans = AppDTO.GetTalkMans(_AppUser.OrgId);

            return View(viewModel);
        }

        // ������Դ
        public IActionResult Public(ResourceSearchParams param, int id = 1)
        {
            Models.Resource.ResourceListViewModel viewModel = new ResourceListViewModel();

            viewModel.Params = param;
            viewModel.StatusList = AppDTO.GetStatusFromJson();
            viewModel.SourceList = AppDTO.GetSourcesFromDb(_AppUser.OrgId);

            // �ͻ��绰�Ƿ�ɼ�
            var roleDataPermissions = this.GetRoleDataPermissions(_AppUser.RoleId);
            viewModel.RoleResourcePhoneVisible = GetRoleResourcePhoneVisible(roleDataPermissions);
            viewModel.CanSearch = roleDataPermissions?.FirstOrDefault(x => x.PermissionId == 5) != null;

            if (param.pi <= 0) param.pi = 1;
            param.ps = _AppConfig.PageSize;
            param.status = 1;

            // ��ҳ����
            var pageOption = new PagerOption {
                PageIndex = param.pi,
                PageSize = param.ps,
                TotalCount = 0,
                RouteUrl = "/Resource/Public",
                QueryString = Request.QueryString.Value
            };

            ViewBag.PagerOption = pageOption;

            // ��ѯ��������
            int totalCount = APIInvoker.Post<int>(_AppConfig.WebApiHost + "/api/Resource/GetResourceCountByCondition", param);
            pageOption.TotalCount = totalCount;

            string searchUrl = _AppConfig.WebApiHost + "/api/Resource/GetResourcesByCondition";
            List<Models.Resource.Resource> resources = APIInvoker.Post<List<Models.Resource.Resource>>(searchUrl, param);
            ResourceDataFormat(resources, viewModel.StatusList, viewModel.SourceList, viewModel.InclinationList, viewModel.RoleResourcePhoneVisible);
            viewModel.Resources = resources;

            // �û�������ҵ����
            ViewBag.UserGroupId = _AppUser.GroupId;

            return View(viewModel);
        }

        // ��������Դ
        public IActionResult Unvalued(ResourceSearchParams param, int id = 1)
        {
            Models.Resource.ResourceListViewModel viewModel = new ResourceListViewModel();

            viewModel.Params = param;
            viewModel.StatusList = AppDTO.GetStatusFromJson();
            viewModel.SourceList = AppDTO.GetSourcesFromDb(_AppUser.OrgId);

            // �ͻ��绰�Ƿ�ɼ�
            var roleDataPermissions = this.GetRoleDataPermissions(_AppUser.RoleId);
            viewModel.RoleResourcePhoneVisible = GetRoleResourcePhoneVisible(roleDataPermissions);
            viewModel.CanSearch = roleDataPermissions?.FirstOrDefault(x => x.PermissionId == 5) != null;
            viewModel.CanDelete = roleDataPermissions?.FirstOrDefault(x => x.PermissionId == 10) != null;

            if (param.pi <= 0) param.pi = 1;
            param.ps = _AppConfig.PageSize;
            param.status = 2;

            // ��ҳ����
            var pageOption = new PagerOption {
                PageIndex = param.pi,
                PageSize = param.ps,
                TotalCount = 0,
                RouteUrl = "/Resource/Unvalued",
                QueryString = Request.QueryString.Value
            };

            ViewBag.PagerOption = pageOption;

            // ��ѯ��������
            int totalCount = APIInvoker.Post<int>(_AppConfig.WebApiHost + "/api/Resource/GetResourceCountByCondition", param);
            pageOption.TotalCount = totalCount;

            string searchUrl = _AppConfig.WebApiHost + "/api/Resource/GetResourcesByCondition";
            List<Models.Resource.Resource> resources = APIInvoker.Post<List<Models.Resource.Resource>>(searchUrl, param);
            ResourceDataFormat(resources, viewModel.StatusList, viewModel.SourceList, viewModel.InclinationList, viewModel.RoleResourcePhoneVisible);
            viewModel.Resources = resources;

            return View(viewModel);
        }

        public IActionResult Signed(ResourceSearchParams param, int id = 1)
        {
            Models.Resource.ResourceListViewModel viewModel = new ResourceListViewModel();

            viewModel.Params = param;
            viewModel.StatusList = AppDTO.GetStatusFromJson();
            viewModel.SourceList = AppDTO.GetSourcesFromDb(_AppUser.OrgId);

            // �ͻ��绰�Ƿ�ɼ�
            var roleDataPermissions = this.GetRoleDataPermissions(_AppUser.RoleId);
            viewModel.RoleResourcePhoneVisible = GetRoleResourcePhoneVisible(roleDataPermissions);
            viewModel.CanSearch = roleDataPermissions?.FirstOrDefault(x => x.PermissionId == 5) != null;

            if (param.pi <= 0) param.pi = 1;
            param.ps = _AppConfig.PageSize;
            param.status = 5;

            // ��ҳ����
            var pageOption = new PagerOption {
                PageIndex = param.pi,
                PageSize = param.ps,
                TotalCount = 0,
                RouteUrl = "/Resource/Signed",
                QueryString = Request.QueryString.Value
            };

            ViewBag.PagerOption = pageOption;

            // ��ѯ��������
            int totalCount = APIInvoker.Post<int>(_AppConfig.WebApiHost + "/api/Resource/GetResourceCountByCondition", param);
            pageOption.TotalCount = totalCount;

            string searchUrl = _AppConfig.WebApiHost + "/api/Resource/GetResourcesByCondition";
            List<Models.Resource.Resource> resources = APIInvoker.Post<List<Models.Resource.Resource>>(searchUrl, param);
            ResourceDataFormat(resources, viewModel.StatusList, viewModel.SourceList, viewModel.InclinationList, viewModel.RoleResourcePhoneVisible);
            viewModel.Resources = resources;

            return View(viewModel);
        }

        // ��Դ¼��
        public IActionResult Add()
        {
            Models.Resource.ResourceViewModel viewModel = new Models.Resource.ResourceViewModel();
            
            // ��ǰ��֯/��˾�µ���Ŀ����
            viewModel.ProjectId = Convert.ToInt32(_AppUser.ProjectId);
            viewModel.Projects = AppDTO.GetProjectsFromDb(_AppUser.OrgId);

            // ����Ⱥ
            viewModel.Inclinations = AppDTO.GetInclinationsFromJson();
            // ��Դ��Դ
            viewModel.Sources = AppDTO.GetSourcesFromDb(_AppUser.OrgId);

            ViewBag.GroupId = _AppUser.GroupId;

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddHandler(Models.Resource.ResourceViewModel viewModel)
        {
            if (viewModel != null) {
                bool result = false;

                int resourceId = InsertResource(viewModel);
                if (resourceId > 0) {
                    // ������Դ����֯����֮��Ĺ�ϵ
                    int resourceOrgId = InsertResourceOrganization(resourceId, _AppUser.OrgId);
                    // ������Դ����Ŀ֮��Ĺ�ϵ
                    int relationId = InsertResourceProject(resourceId, viewModel.ProjectId);
                    // ��Դ����
                    if (viewModel.ResourceBelong == 1 && _AppUser.GroupId != null && _AppUser.GroupId > 0) {
                        // �����Լ�
                        int resourceGroupId = InsertResourceGroup(resourceId, (int)_AppUser.GroupId);
                        int resourceUserId = InsertResourceUser(resourceId, _AppUser.Id);
                        // ������Դ״̬Ϊ'Ǣ̸��'
                        SetResourceStatus(resourceId, 4);
                    }
                    else {
                        // ����'δ����'
                        SetResourceStatus(resourceId, 3);
                    }
                    result = true;
                }
                
                TempData["result"] = result;
                return RedirectToAction("Add");
            }
            return View();
        }

        // ��Դ��������
        public IActionResult BatchImport()
        {
            return View();
        }

        // �ͷ���Ա��"��������"���ò������䵽ĳ��ҵ����
        public IActionResult Assign()
        {
            ViewBag.ProjectList = AppDTO.GetProjectsFromDb(_AppUser.OrgId);
            ViewBag.ProjectId = _AppUser.ProjectId;
            if (_AppUser.ProjectId != null && _AppUser.ProjectId > 0) {
                ViewBag.GroupList = AppDTO.GetGroupsByProjectId((int)_AppUser.ProjectId);
            }
            // ��Դ��ѯȨ��
            var roleDataPermissions = this.GetRoleDataPermissions(_AppUser.RoleId);
            ViewBag.CanSearch = roleDataPermissions?.FirstOrDefault(x => x.PermissionId == 5) != null;

            return View();
        }

        [HttpPost]
        public IActionResult AssignHandler()
        {
            int projectId = Convert.ToInt32(Request.Form["project"]);
            if (projectId > 0) {
                string assignInfo = Request.Form["hidAssignInfo"];
                var type = new[] {
                    new {
                        groupId=0,
                        assignCount=0
                    }
                };

                var assignInfoList = JsonConvert.DeserializeAnonymousType(assignInfo, type);
                if(assignInfoList != null && assignInfoList.Length > 0) {
                    // ��ȡ��δ�������Դ
                    string unAssignedResourcesApi = _AppConfig.WebApiHost + $"/api/Resource/GetGroupUnAssignedResources?projectId={projectId}";
                    List<UnassignedResource> resources = APIInvoker.Get<List<UnassignedResource>>(unAssignedResourcesApi);

                    List<ResourceGroup> resourceGroups = new List<ResourceGroup>();//�����ϵResourceGroup����
                    // ��ʼ����
                    int i = 0;
                    foreach (var item in assignInfoList) {
                        if (item.assignCount > 0) {
                            //ÿ�η�������������ܳ���ʣ������(assignCount��ʾ��ǰ��������������resources.Count-i��ʾʣ������)
                            int spliceCount = Math.Min(item.assignCount, resources.Count - i);//����ʵ�ʷ�������
                            var blocks = resources.GetRange(i, spliceCount);//�г������ݿ�
                            i += spliceCount;
                            // ���ɷ����ϵResourceGroup
                            if(blocks != null && blocks.Count > 0) {
                                DateTime now = DateTime.Now;
                                foreach(var resource in blocks) {
                                    resourceGroups.Add(new ResourceGroup() { ResourceId = resource.ResourceId, GroupId = item.groupId, CreateTime = now });
                                }
                            }
                        }
                    }
                    // �������ݿ�
                    foreach(var item in resourceGroups) {
                        InsertResourceGroup(item.ResourceId, item.GroupId);
                    }
                }
                TempData["result"] = true;
                return RedirectToAction("Assign");
            }
            return View();
        }

        // ��Դ��Դͳ��
        public IActionResult SourceStat()
        {
            string apiUrl = _AppConfig.WebApiHost + "/api/Resource/GetResourceSourceStat?orgId=" + _AppUser.OrgId;
            var list = APIInvoker.Get<List<Models.Resource.ResourceSource>>(apiUrl);
            ViewBag.SourceStat = JsonConvert.SerializeObject(list);

            return View();
        }

        /// <summary>
        /// ��Դ/�ͻ���ϸ��Ϣ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Detail(int id)
        {
            ResourceDetailViewModel viewModel = new ResourceDetailViewModel();

            string resourceApiUrl = _AppConfig.WebApiHost + "/api/Resource/GetResourceById?id=" + id;
            Models.Resource.Resource resource = APIInvoker.Get<Models.Resource.Resource>(resourceApiUrl);

            if (resource == null)
                return RedirectToAction("Http404", "Error");

            // �ͻ�(��Դ)��Ϣ
            viewModel.ResourceId = id;
            viewModel.CustomerName = resource.CustomerName?.Trim();
            viewModel.Mobile = resource.Mobile?.Trim();
            viewModel.QQ = resource.QQ?.Trim();
            viewModel.Wechat = resource.Wechat?.Trim();
            viewModel.Tel = resource.Tel?.Trim();
            viewModel.Email = resource.Email;
            viewModel.SourceFrom = resource.SourceFrom;
            viewModel.Status = resource.Status;
            viewModel.Inclination = resource.Inclination;
            viewModel.Sex = resource.Sex;
            viewModel.Address = resource.Address;
            viewModel.Remark = resource.Remark?.Trim();
            // ��ǩ
            string apiNote = _AppConfig.WebApiHost + "/api/ResourceNote/GetNotesByResourceId?resourceId=" + id;
            viewModel.ResourceNotes = APIInvoker.Get<List<Models.Resource.ResourceNote>>(apiNote);

            // Ǣ̸��¼
            string apiRecord = _AppConfig.WebApiHost + "/api/TalkRecord/GetRecordsByResourceId?resourceId=" + id;
            viewModel.TalkRecords = APIInvoker.Get<List<Models.Resource.TalkRecord>>(apiRecord);

            // ǩԼ��¼
            string apiSign = _AppConfig.WebApiHost + "/api/CustomerSign/GetSignByResourceId?resourceId=" + id;
            viewModel.Sign = APIInvoker.Get<Models.Sign.CustomerSign>(apiSign);

            // ��Դ״̬&����&��Դ
            viewModel.StatusList = AppDTO.GetStatusFromJson();
            viewModel.InclinationList = AppDTO.GetInclinationsFromJson();
            viewModel.SourceList = AppDTO.GetSourcesFromDb(_AppUser.OrgId);

            // ��Դ�༭Ȩ��
            var resourceHandlePermissions = GetResourceHandlePermissions(_AppUser.RoleId);
            var editPermission = resourceHandlePermissions.FirstOrDefault(x => x.PermissionId == 8);
            if (editPermission != null) {
                viewModel.ResourceEdit = true;
            }

            // �ͻ��绰Ȩ��
            var roleDataPermissions = this.GetRoleDataPermissions(_AppUser.RoleId);
            bool phoneVisible = GetRoleResourcePhoneVisible(roleDataPermissions);
            if (!phoneVisible) {
                viewModel.Mobile = AppDTO.EncryptPhone(viewModel.Mobile);
                viewModel.Tel = AppDTO.EncryptPhone(viewModel.Tel);
            }

            // ̸����
            viewModel.TalkMans = AppDTO.GetTalkMans(_AppUser.OrgId);


            return View(viewModel);
        }

        [HttpPost]
        public IActionResult DetailHandler(Models.Resource.ResourceDetailViewModel viewModel)
        {
            if (viewModel != null && viewModel.ResourceId > 0) {
                string resourceApiUrl = _AppConfig.WebApiHost + "/api/Resource/GetResourceById?id=" + viewModel.ResourceId;
                Models.Resource.Resource resource = APIInvoker.Get<Models.Resource.Resource>(resourceApiUrl);
                if (resource != null) {
                    // ����޸�֮ǰ��״̬����1��2��������1��2����˵����Դ�����빫��������⣬��ʱ��Ǣ̸��¼�������־
                    if (resource.Status != 1 && resource.Status != 2 && (viewModel.Status == 1 || viewModel.Status ==2)) {
                        // д��Ǣ̸��¼
                        string talkResult = _AppUser.RealName + "������Դ����" + (viewModel.Status == 1 ? "������" : "������");
                        AddTalkRecord(viewModel.ResourceId, 5, talkResult, _AppUser.Id, 1);
                    }

                    // ��Դ����
                    resource.CustomerName = viewModel.CustomerName?.Trim();
                    if (viewModel.Mobile == null || !viewModel.Mobile.Contains("*")) {
                        resource.Mobile = viewModel.Mobile?.Trim();
                    }
                    resource.Wechat = viewModel.Wechat?.Trim();
                    resource.QQ = viewModel.QQ?.Trim();
                    if(viewModel.Tel == null || !viewModel.Tel.Contains("*")) { 
                        resource.Tel = viewModel.Tel?.Trim();
                    }
                    resource.SourceFrom = viewModel.SourceFrom;
                    resource.Sex = viewModel.Sex;
                    resource.Inclination = viewModel.Inclination;
                    resource.Address = viewModel.Address;
                    resource.Status = viewModel.Status;
                    resource.Remark = viewModel.Remark?.Trim();
                    resource.UpdateTime = DateTime.Now;

                    // ����
                    string resourceUpdateApiUrl = _AppConfig.WebApiHost + "/api/Resource/UpdateReource";
                    bool result = APIInvoker.Post<bool>(resourceUpdateApiUrl, resource);
                    TempData["result"] = result;

                    // �������Դ���빫���⡢�����⡢δ��������Ҫɾ��Resource��Group��User�İ󶨹�ϵ
                    // 1=������2=������3=δ���䣬4=Ǣ̸�У�5=��ǩԼ��6=�����ã�7=��ͨ�����죬8=��ͨ���˽⣬9=��ͨ������[���У�1-3���ڹ�����Դ��4-9���ڸ���]
                    if (viewModel.Status == 1 || viewModel.Status == 2 || viewModel.Status == 3) { 
                        // ɾ��ResourceGroup
                        string rgDeleteApiUrl = _AppConfig.WebApiHost + "/api/Resource/DeleteResourceGroupByResourceIds?resourceIds=" + viewModel.ResourceId;
                        int count1 = APIInvoker.Get<int>(rgDeleteApiUrl);

                        // ɾ��ResourceUser
                        string ruDeleteApiUrl = _AppConfig.WebApiHost + "/api/Resource/BatchDeleteResourceUser?resourceIds=" + viewModel.ResourceId;
                        int count2 = APIInvoker.Get<int>(ruDeleteApiUrl);
                    }

                    return RedirectToAction("Detail", new { id = viewModel.ResourceId });
                }
            }
            return RedirectToAction("Index", "Error");
        }

        // �ͻ���ѯ����
        public IActionResult Search(CustomerSearchViewModel viewModel)
        {
            // ��Դ��ѯȨ��
            var roleDataPermissions = this.GetRoleDataPermissions(_AppUser.RoleId);
            ViewBag.CanSearch = roleDataPermissions?.FirstOrDefault(x => x.PermissionId == 5) != null;

            if (string.IsNullOrEmpty(viewModel.key)) return View(viewModel);

            // ִ�в�ѯ
            string resourceApiUrl = _AppConfig.WebApiHost + "/api/Resource/GetResourceByNameMobileWechatQQ?key=" + viewModel.key + "&orgId=" + _AppUser.OrgId;
            Models.Resource.Resource resource = APIInvoker.Get<Models.Resource.Resource>(resourceApiUrl);

            if (resource != null) {
                // �ͻ�(��Դ)��Ϣ
                viewModel.Id = resource.Id;
                viewModel.CustomerName = resource.CustomerName;
                viewModel.Mobile = resource.Mobile;
                viewModel.QQ = resource.QQ;
                viewModel.Wechat = resource.Wechat;
                viewModel.Tel = resource.Tel;
                viewModel.Email = resource.Email;
                viewModel.SourceFromText = AppDTO.GetSourceDisplayText(resource.SourceFrom, _AppUser.OrgId);
                viewModel.StatusText = AppDTO.GetStatusDisplayText(resource.Status);
                viewModel.InclinationText = AppDTO.GetInclinationDisplayText(resource.Inclination);
                viewModel.SexText = AppDTO.GetSexDisplayText(resource.Sex);
                viewModel.Address = resource.Address;
                viewModel.Remark = resource.Remark;

                // Ǣ̸��¼
                string apiRecord = _AppConfig.WebApiHost + "/api/TalkRecord/GetRecordsByResourceId?resourceId=" + resource.Id;
                viewModel.TalkRecords = APIInvoker.Get<List<Models.Resource.TalkRecord>>(apiRecord);

                // ǩԼ��¼
                string apiSign = _AppConfig.WebApiHost + "/api/CustomerSign/GetSignByResourceId?resourceId=" + resource.Id;
                viewModel.Sign = APIInvoker.Get<Models.Sign.CustomerSign>(apiSign);

                // ��֯�����µ�ҵ��Ա
                string apiUser = _AppConfig.WebApiHost + "/api/AppUser/GetAllUsersByProjectId?projectId=" + resource.ProjectId;
                viewModel.OrgUsers = APIInvoker.Get<List<Models.AppUser.AppUserComplex>>(apiUser);

                // �ͻ��绰Ȩ��
                bool phoneVisible = GetRoleResourcePhoneVisible(roleDataPermissions);
                if (!phoneVisible) {
                    viewModel.Mobile = AppDTO.EncryptPhone(viewModel.Mobile);
                    viewModel.Tel = AppDTO.EncryptPhone(viewModel.Tel);
                }
            }

            return View(viewModel);
        }

        // ����ͻ�ǩԼ��¼
        public bool InsertCustomerSign(Models.Sign.CustomerSign sign)
        {
            if (sign != null && sign.Amount > 0) {
                sign.AppendUserId = _AppUser.Id;
                sign.AppendMan = _AppUser.RealName;
                sign.OrgId = _AppUser.OrgId;
                sign.CreateTime = DateTime.Now;

                // ��ȡǩԼ�û���������������
                string userGetApi = _AppConfig.WebApiHost + "/api/AppUser/GetUserById?id=" + sign.SignUserId;
                var appUser = APIInvoker.Get<Models.AppUser.AppUserViewModel>(userGetApi);
                if (appUser != null) {
                    sign.SignMan = appUser.RealName;//ǩԼ�û�����
                    sign.GroupId = Convert.ToInt32(appUser.GroupId);//ǩԼ�û�������Id
                    sign.GroupName = appUser.GroupName;//ǩԼ�û�����������
                }

                string signApiUrl = _AppConfig.WebApiHost + "/api/CustomerSign/InsertSign";
                bool result = APIInvoker.Post<bool>(signApiUrl, sign);

                TempData["result"] = result;

                return result;
            }
            return false;
        }

        #region ������Դ InsertResource
        private int InsertResource(Models.Resource.ResourceViewModel viewModel)
        {
            var resource = new
            {
                CustomerName = viewModel.CustomerName?.Trim(),
                Message = viewModel.Message?.Trim(),
                Mobile = viewModel.Mobile?.Trim(),
                Tel = viewModel.Tel?.Trim(),
                Wechat = viewModel.Wechat?.Trim(),
                QQ = viewModel.QQ?.Trim(),
                Email = viewModel.Email,
                Address = viewModel.Province + viewModel.City + viewModel.Address,
                Inclination = viewModel.Inclination,
                TalkCount = 0,
                SourceFrom = viewModel.SourceFrom,
                Status = 3,
                AppendUserId = _AppUser.Id,
                Remark = viewModel.Remark?.Trim(),
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                DeleteFlag = 0
            };

            string apiUrl = _AppConfig.WebApiHost + "/api/Resource/InsertResource";
            int identityId = APIInvoker.Post<int>(apiUrl, resource);

            return identityId;
        }
        #endregion

        #region ������Դ����֯����֮��Ĺ�ϵ
        private int InsertResourceOrganization(int resourceId, int orgId)
        {
            var resourceOrg = new
            {
                ResourceId = resourceId,
                OrgId = orgId,
                CreateTime = DateTime.Now
            };

            string apiUrl = _AppConfig.WebApiHost + "/api/Resource/InsertResourceOrganization";
            int identityId = APIInvoker.Post<int>(apiUrl, resourceOrg);

            return identityId;
        }
        #endregion

        #region ������Դ����Ŀ�Ĺ�ϵ
        private int InsertResourceProject(int resourceId, int projectId)
        {
            var resourceProject = new
            {
                ResourceId = resourceId,
                ProjectId = projectId,
                CreateTime = DateTime.Now
            };

            string apiUrl = _AppConfig.WebApiHost + "/api/Resource/InsertResourceProject";
            int identityId = APIInvoker.Post<int>(apiUrl, resourceProject);

            return identityId;
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

            string apiUrl = _AppConfig.WebApiHost + "/api/Resource/InsertResourceGroup";
            int identityId = APIInvoker.Post<int>(apiUrl, resourceGroup);

            return identityId;
        }
        #endregion

        #region ������Դ���û��Ĺ�ϵ
        private int InsertResourceUser(int resourceId, int userId)
        {
            var resourceUser = new
            {
                ResourceId = resourceId,
                UserId = userId,
                CreateTime = DateTime.Now
            };

            string apiUrl = _AppConfig.WebApiHost + "/api/Resource/InsertResourceUser";
            int identityId = APIInvoker.Post<int>(apiUrl, resourceUser);

            return identityId;
        }
        #endregion

        #region ��Դ�б����ݵļӹ�����
        /// <summary>
        /// ��Դ���ݵļӹ�����
        /// </summary>
        /// <param name="resources"></param>
        /// <param name="status"></param>
        /// <param name="sources"></param>
        /// <param name="inclinations"></param>
        /// <param name="phoneVisible">�ͻ��绰�Ƿ�ɼ�</param>
        public void ResourceDataFormat(List<Models.Resource.Resource> resources, List<SelectItem> status, List<Models.Source.Source> sources, List<SelectItem> inclinations,
            bool phoneVisible)
        {
            if (resources != null) {
                foreach (var resource in resources) {
                    // ��ϵ��ʽContactInfo�Ĵ���
                    string contactInfo = "";
                    if (!string.IsNullOrEmpty(resource.Mobile)) {
                        contactInfo = $"[��:{resource.Mobile}]";
                    }
                    else if (!string.IsNullOrEmpty(resource.Tel)) {
                        contactInfo = $"[��:{resource.Tel}]";
                    }
                    else if (!string.IsNullOrEmpty(resource.QQ)) {
                        contactInfo = $"[QQ:{resource.QQ}]";
                    }
                    else if (!string.IsNullOrEmpty(resource.Wechat)) {
                        contactInfo = $"[΢��:{resource.Wechat}]";
                    }
                    else if (!string.IsNullOrEmpty(resource.Email)) {
                        contactInfo = $"[Email:{resource.Email}]";
                    }
                    if (!phoneVisible) {
                        contactInfo = AppDTO.EncryptContactInfo(contactInfo);
                    }
                    resource.ContactInfo = contactInfo;
                    // ��Դ
                    if (resource.SourceFrom != null && sources != null) {
                        var query = sources.FirstOrDefault(x => x.Id == resource.SourceFrom);
                        if (query != null) {
                            resource.SourceFromText = query.SourceName;
                        }
                    }
                    // ����
                    if (resource.Inclination != null && inclinations != null) {
                        var query = inclinations.FirstOrDefault(x => x.value == resource.Inclination);
                        if (query != null) {
                            resource.InclinationText = query.displayText;
                        }
                    }
                    // ״̬
                    if (resource.Status != null && status != null) {
                        var query = status.FirstOrDefault(x => x.value == resource.Status);
                        if (query != null) {
                            resource.StatusText = query.displayText;
                        }
                    }
                }
            }
        }
        #endregion

        #region ������Դ���� ResourceAssign
        /// <summary>
        /// ��Դ����
        /// ��Դ��ҵ���飬��Դ���û�����һ��һ��ϵ
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="groupId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public bool ResourceAssign(int resourceId, int groupId, int userId)
        {
            if (resourceId == 0 || groupId == 0 || userId == 0) return false;

            bool assignResult = true;
            
            string rgGetAPI = _AppConfig.WebApiHost + "/api/Resource/GetResourceGroup?resourceId=" + resourceId;
            string rgUpdateAPI = _AppConfig.WebApiHost + "/api/Resource/UpdateResourceGroup";
            string rgInsertAPI = _AppConfig.WebApiHost + "/api/Resource/InsertResourceGroup";

            // ����ResourceGroup��
            Models.Resource.ResourceGroup resourceGroup = APIInvoker.Get<Models.Resource.ResourceGroup>(rgGetAPI);
            if (resourceGroup == null) {
                // insert
                var item = new
                {
                    ResourceId = resourceId,
                    GroupId = groupId,
                    CreateTime = DateTime.Now
                };
                bool result = APIInvoker.Post<bool>(rgInsertAPI, item);
                if (!result) assignResult = false;
            }
            else {
                // update
                var item = new
                {
                    ResourceId = resourceId,
                    GroupId = groupId
                };
                bool result = APIInvoker.Post<bool>(rgUpdateAPI, item);
                if (!result) assignResult = false;
            }

            // ����ResourceUser��
            string ruGetAPI = _AppConfig.WebApiHost + "/api/Resource/GetResourceUser?resourceId=" + resourceId;
            string ruUpdateAPI = _AppConfig.WebApiHost + "/api/Resource/UpdateResourceUser";
            string ruInsertAPI = _AppConfig.WebApiHost + "/api/Resource/InsertResourceUser";
            Models.Resource.ResourceUser resourceUser = APIInvoker.Get<Models.Resource.ResourceUser>(ruGetAPI);
            if (resourceUser == null) {
                // insert
                var item = new
                {
                    ResourceId = resourceId,
                    UserId = userId,
                    CreateTime = DateTime.Now
                };
                bool result = APIInvoker.Post<bool>(ruInsertAPI, item);
                if (!result) assignResult = false;
            }
            else {
                // update
                var item = new
                {
                    ResourceId = resourceId,
                    UserId = userId
                };
                bool result = APIInvoker.Post<bool>(ruUpdateAPI, item);
                if (!result) assignResult = false;
            }

            // ���Ǣ̸��¼(��Դ����Type=1)
            string userGetApi = _AppConfig.WebApiHost + "/api/AppUser/GetUserById?id=" + userId;
            Models.AppUser.AppUserViewModel targetUser = APIInvoker.Get<Models.AppUser.AppUserViewModel>(userGetApi);
            if (targetUser != null) { 
                string talkResult = _AppUser.RealName + "������Դ�����" + targetUser.RealName;
                AddTalkRecord(resourceId, 5, talkResult, _AppUser.Id, 1);
            }

            // ������Դ״̬ΪǢ̸��
            SetResourceStatus(resourceId, 4);

            return assignResult;
        }
        #endregion

        #region ������Դ״̬
        private bool SetResourceStatus(int resourceId, int status)
        {
            string apiUrl = _AppConfig.WebApiHost + $"/api/Resource/SetResourceStatus?resourceId={resourceId}&status={status}";

            bool result = APIInvoker.Get<bool>(apiUrl);
            return result;
        } 
        #endregion

        #region ɾ����Դ
        /// <summary>
        /// ɾ����Դ ajax
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public bool DeleteResource(int id)
        {
            string apiUrl = _AppConfig.WebApiHost + "/api/Resource/DeleteResource?id=" + id;
            bool result = APIInvoker.Get<bool>(apiUrl);
            return result;
        }
        #endregion

        #region ��ԭ��Դ
        [HttpGet]
        public bool RestoreResource(int id)
        {
            return SetResourceStatus(id, 1);//��ԭ��������
        } 
        #endregion

        #region ��Դ�Ƿ����
        public bool IsExist(string mobile, string tel, string qq, string wechat)
        {
            string apiUrl = _AppConfig.WebApiHost + $"/api/Resource/IsResourceExist?orgId={_AppUser.OrgId}&mobile={mobile}&tel={tel}&qq={qq}&wechat={wechat}";
            bool result = APIInvoker.Get<bool>(apiUrl);
            return result;
        } 
        #endregion

        #region ���һ��Ǣ̸��¼
        /// <summary>
        /// ���һ��Ǣ̸��¼
        /// </summary>
        /// <param name="resourceId">��ԴId</param>
        /// <param name="talkWay">Ǣ̸��ʽ</param>
        /// <param name="talkResult">Ǣ̸С��</param>
        /// <param name="userId">Ǣ̸��</param>
        /// <param name="type">����(0=�û���ӣ�1=ϵͳ��Դ����Ȳ���)</param>
        /// <returns></returns>
        [HttpPost]
        public bool AddTalkRecord(int resourceId, int talkWay, string talkResult, int userId, int type = 0)
        {
            // ����Ԥ����
            if(userId <= 0) {
                if (talkWay == 3) return false; // û��̸��������
                else userId = _AppUser.Id;
            }
            // �������ݿ�
            string apiUrl = _AppConfig.WebApiHost + "/api/TalkRecord/InsertTalkRecord";
            var record = new
            {
                ResourceId = resourceId,
                TalkWay = talkWay,
                TalkResult = talkResult,
                UserId = userId,
                Type = type,//0=Ĭ�ϣ���ʾ�û���ӵģ�1=��Դ�������
                CreateTime = DateTime.Now
            };

            bool result = APIInvoker.Post<bool>(apiUrl, record);
            return result;
        }
        #endregion

        #region ɾ��һ��Ǣ̸��¼
        public bool DelTalkRecord(int id, int resourceId)
        {
            if (id <= 0) return false;
            string apiUrl = _AppConfig.WebApiHost + "/api/TalkRecord/DeleteTalkRecord?id=" + id + "&resourceId=" + resourceId;
            bool result = APIInvoker.Get<bool>(apiUrl);
            return result;
        } 
        #endregion

        /// <summary>
        /// ��ȡ��ɫ�µ�����Ȩ��
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        private List<Models.Role.RoleDataPermission> GetRoleDataPermissions(int roleId)
        {
            string apiUrl = _AppConfig.WebApiHost + "/api/DataPermission/GetRoleDataPermissions?roleId=" + roleId;
            var dataResult = APIInvoker.Get<List<Models.Role.RoleDataPermission>>(apiUrl);
            return dataResult;
        }

        /// <summary>
        /// ��ɫ������Ȩ�ޣ���ȡ��Դ�ɼ���Χ
        /// ��Դ�ɼ���Χ��4����������Դ=1��������Դ=2������Ŀ��Դ=3����˾��Դ=4
        /// </summary>
        /// <param name="roleDataPermissions"></param>
        /// <returns></returns>
        private int GetRoleResourceVisible(List<Models.Role.RoleDataPermission> roleDataPermissions)
        {
            if (roleDataPermissions == null) return -1;

            var query = roleDataPermissions.FirstOrDefault(x => x.PermissionCategoryId == 1);
            if (query != null) {
                return query.PermissionId;
            }
            return -1;
        }

        /// <summary>
        /// ��ɫ������Ȩ�ޣ���ȡ�ͻ��绰�Ƿ�ɼ�
        /// True=�ɼ���False=���ɼ�
        /// </summary>
        /// <param name="roleDataPermissions"></param>
        /// <returns></returns>
        private bool GetRoleResourcePhoneVisible(List<Models.Role.RoleDataPermission> roleDataPermissions)
        {
            if (roleDataPermissions == null) return false;

            var query = roleDataPermissions.FirstOrDefault(x => x.PermissionCategoryId == 3 && x.PermissionId == 9);
            if (query != null) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// ��ɫ������Ȩ�ޣ���Դ����
        /// ��Դ����Ȩ����4������Դ��ѯ=5����Դ����=6����Դ��������=7����Դ�༭=8
        /// </summary>
        /// <param name="roleId">��ɫid</param>
        /// <returns></returns>
        private List<Models.Role.RoleDataPermission> GetResourceHandlePermissions(int roleId)
        {
            if (roleId > 0) {
                var dataPermissions = GetRoleDataPermissions(roleId);
                if(dataPermissions != null) { 
                    var resourceHandlePermissions = dataPermissions.Where(x => x.PermissionCategoryId == 2).ToList();//��Դ����Ȩ��
                    return resourceHandlePermissions;
                }
            }
            return null;
        }

        /// <summary>
        /// ��ɫ������Ȩ�ޣ���ȡ��Դ����Ȩ��
        /// </summary>
        /// <param name="roleDataPermissions"></param>
        /// <returns></returns>
        private List<Models.Role.RoleDataPermission> GetRoleResourceHandle(List<Models.Role.RoleDataPermission> roleDataPermissions)
        {
            if (roleDataPermissions == null) return null;

            var query = roleDataPermissions.Where(x => x.PermissionCategoryId == 2).ToList();
            return query;
        }

        /// <summary>
        /// ��ȡδ������ҵ�������Դ����
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public int GetGroupUnAssignedResourceCount(int projectId)
        {
            string apiUrl = _AppConfig.WebApiHost + "/api/Resource/GetGroupUnAssignedResourceCount?projectId=" + projectId;
            int count = APIInvoker.Get<int>(apiUrl);
            return count;
        }

        /// <summary>
        /// ��ȡ�û��Զ���ı�ǩ
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private List<Models.Tag.Tag> GetTagList(int userId)
        {
            string apiUrl = _AppConfig.WebApiHost + "/api/Tag/GetTagsByUserId?userId=" + userId;
            var tags = APIInvoker.Get<List<Models.Tag.Tag>>(apiUrl);
            return tags;
        }

        /// <summary>
        /// ��Դ�б�ҳ��"��������",�ò������䵽ĳ��ҵ��Ա
        /// </summary>
        /// <param name="resourceIds"></param>
        /// <param name="groupId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public bool ResourceBatchAssign(string resourceIds, int groupId, int userId)
        {
            string apiUrl = _AppConfig.WebApiHost + $"/api/Resource/ResourceBatchAssign?resourceIds={resourceIds}&groupId={groupId}&userId={userId}&operatorId={_AppUser.Id}";
            bool result = APIInvoker.Get<bool>(apiUrl);
            return result;
        }

        [HttpGet]
        public int DivideToMe(int resourceId)
        {
            // ���ȼ�⵱ǰ��Դ�Ƿ��ѱ�����(���Ⲣ���γ�������)
            string apiUrl = _AppConfig.WebApiHost + "/api/Resource/GetResourceUser?resourceId=" + resourceId;
            ResourceUser resourceUser = APIInvoker.Get<ResourceUser>(apiUrl);

            // ִ�з���
            if (resourceUser == null) {
                apiUrl = _AppConfig.WebApiHost + $"/api/Resource/DivideToMe?resourceId={resourceId}&groupId={_AppUser.GroupId}&userId={_AppUser.Id}";
                bool result = APIInvoker.Get<bool>(apiUrl);
                return result ? 1 : 0;
            }
            return -1;
        }

        #region ������Դ�������ָ��Լ�
        [HttpPost]
        public int BatchDivideToMe()
        {
            if (_AppUser.GroupId != null && _AppUser.GroupId > 0 && _AppUser.Id != 0) {
                string resourceIds = Request.Form["resourceIds"];
                if (string.IsNullOrEmpty(resourceIds)) return -1; // ��������

                // ���ȼ�⵱ǰ��Դ�Ƿ��ѱ�����(���Ⲣ���γ�������)
                string apiUrl = _AppConfig.WebApiHost + $"/api/Resource/GetResourceGroupsByResourceIds?resourceIds={resourceIds}";
                List<ResourceGroup> existResourceGroups = APIInvoker.Get<List<ResourceGroup>>(apiUrl);
                apiUrl = _AppConfig.WebApiHost + $"/api/Resource/GetResourceUsersByResourceIds?resourceIds={resourceIds}";
                List<ResourceUser> existResourceUsers = APIInvoker.Get<List<ResourceUser>>(apiUrl);

                List<ResourceGroup> resourceGroups = new List<ResourceGroup>();
                List<ResourceUser> resourceUsers = new List<ResourceUser>();

                string[] resourceIdArr = resourceIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (resourceIdArr != null && resourceIdArr.Length > 0) {
                    foreach (string resourceIdStr in resourceIdArr) {
                        int resourceId = int.Parse(resourceIdStr);

                        ResourceGroup rg = existResourceGroups?.FirstOrDefault(x => x.ResourceId == resourceId);
                        if (rg != null) {
                            if (rg.GroupId == _AppUser.GroupId) {
                                // ����Դ�ѱ���ǰ�����У���δָ������ӵ����
                                ResourceUser ru = existResourceUsers?.FirstOrDefault(x => x.ResourceId == resourceId);
                                if (ru == null) {
                                    resourceUsers.Add(new ResourceUser() { ResourceId = resourceId, UserId = _AppUser.Id, CreateTime = DateTime.Now });
                                }
                            }
                            else {
                                // ����Դ�ѱ���������䣬���봦��
                            }
                        }
                        else {
                            ResourceUser ru = existResourceUsers?.FirstOrDefault(x => x.ResourceId == resourceId);
                            if (ru == null) {
                                resourceGroups.Add(new ResourceGroup() { ResourceId = resourceId, GroupId = Convert.ToInt32(_AppUser.GroupId), CreateTime = DateTime.Now });
                                resourceUsers.Add(new ResourceUser() { ResourceId = resourceId, UserId = _AppUser.Id, CreateTime = DateTime.Now });
                            }
                            else {
                                if (ru.UserId == _AppUser.Id) {
                                    resourceGroups.Add(new ResourceGroup() { ResourceId = resourceId, GroupId = Convert.ToInt32(_AppUser.GroupId), CreateTime = DateTime.Now });
                                }
                                else {
                                    // ������Դ�ѱ�ĳһ�û����У����Ǹ��û�������Ϊ�գ����������ݣ��ݲ�����
                                }
                            }
                        }
                    }
                }

                // ִ�з���
                if (resourceGroups != null && resourceGroups.Count > 0) {
                    apiUrl = _AppConfig.WebApiHost + "/api/Resource/ResourceGroupBatchInsert";
                    bool res1 = APIInvoker.Post<bool>(apiUrl, resourceGroups);
                }
                int count = 0;
                if (resourceUsers != null && resourceUsers.Count > 0) {
                    apiUrl = _AppConfig.WebApiHost + "/api/Resource/ResourceUserBatchInsert";
                    bool res2 = APIInvoker.Post<bool>(apiUrl, resourceUsers);
                    count = res2 ? resourceUsers.Count : 0;

                    if (res2) {
                        // ������Щ��Դ��״̬Ϊ"Ǣ̸��=4"
                        apiUrl = _AppConfig.WebApiHost + $"/api/Resource/BatchSetResourceStatus?resourceIds={resourceIds}&status={4}";
                        int n = APIInvoker.Get<int>(apiUrl);
                        // ��¼������־
                        List<Models.Resource.TalkRecord> talkRecords = new List<TalkRecord>();
                        foreach (var _ru in resourceUsers) {
                            Models.Resource.TalkRecord talkRecord = new Models.Resource.TalkRecord();
                            talkRecord.ResourceId = _ru.ResourceId;
                            talkRecord.TalkWay = 5;
                            talkRecord.TalkResult = _AppUser.RealName + "������Դ�ӹ����⻮�ָ��Լ�";
                            talkRecord.UserId = _ru.UserId;
                            talkRecord.Type = 1;
                            talkRecord.CreateTime = DateTime.Now;

                            talkRecords.Add(talkRecord);
                        }
                        apiUrl = _AppConfig.WebApiHost + "/api/TalkRecord/TalkRecordBatchInsert";
                        bool res3 = APIInvoker.Post<bool>(apiUrl, talkRecords);
                    }
                }
                return count;
            }
            else {
                return -2;// ����������ĳ��ҵ���������ִ�д˲�����
            }
        } 
        #endregion
    }
}