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

namespace Orion.CRM.WebApp.Controllers
{
    /// <summary>
    /// 资源管理控制器
    /// </summary>
    public class ResourceController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnv;
        public ResourceController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnv = hostingEnvironment;
        }

        // 资源列表
        public IActionResult List(ResourceSearchParams param)
        {
            Models.Resource.ResourceListViewModel viewModel = new ResourceListViewModel();
            viewModel.Params = param;

            var roleDataPermissions = this.GetRoleDataPermissions(_AppUser.RoleId);
            
            viewModel.StatusList = AppDTO.GetStatusFromJson();
            viewModel.InclinationList = AppDTO.GetInclinationsFromJson();
            viewModel.SourceList = AppDTO.GetSourcesFromDb(_AppUser.OrgId);
            viewModel.TalkCountList = AppDTO.GetTalkCountFromJson();
            viewModel.RoleResourceVisible = GetRoleResourceVisible(roleDataPermissions);
            viewModel.RoleResourceHandle = GetRoleResourceHandle(roleDataPermissions);
            viewModel.ProjectId = _AppUser.ProjectId;

            if (param.pi <= 0) param.pi = 1;
            param.ps = _AppConfig.PageSize;

            // 分页参数
            var pageOption = new PagerOption {
                PageIndex = param.pi,
                PageSize = param.ps,
                TotalCount = 0,
                RouteUrl = "/Resource/List",
                QueryString = Request.QueryString.Value
            };

            string groupApiUrl = _AppConfig.WebApiHost + "api/Group/GetGroupsByProjectId?projectId=";
            string groupMemberApiUrl = _AppConfig.WebApiHost + "api/AppUser/GetAllUsersByGroupId?groupId=";

            switch (viewModel.RoleResourceVisible) {
                case 4:
                    // 资源可见范围：公司资源，需加载[项目列表、业务组列表、业务员列表]
                    viewModel.ProjectList = AppDTO.GetProjectsFromDb(_AppUser.OrgId);
                    //viewModel.GroupList = APIInvoker.Get<List<Models.Group.Group>>(groupApiUrl+_AppUser.ProjectId);
                    //viewModel.SalerList = APIInvoker.Get<List<Models.AppUser.AppUserViewModel>>(apiUser);
                    break;
                case 3:
                    // 资源可见范围：本项目资源，需加载[业务组列表、业务员列表]
                    viewModel.GroupList = APIInvoker.Get<List<Models.Group.Group>>(groupApiUrl + _AppUser.ProjectId);

                    //viewModel.SalerList = APIInvoker.Get<List<Models.AppUser.AppUserViewModel>>(apiUser);
                    param.pid = _AppUser.ProjectId;
                    break;
                case 2:
                    // 资源可见范围：本组资源，需加载[业务员列表]
                    //viewModel.SalerList = APIInvoker.Get<List<Models.AppUser.AppUserViewModel>>(apiUser);
                    param.pid = _AppUser.ProjectId;
                    param.gid = _AppUser.GroupId;
                    viewModel.SalerList = APIInvoker.Get<List<Models.AppUser.AppUserViewModel>>(groupMemberApiUrl + param.gid);
                    break;
                case 1:
                    // 资源可见范围：本人资源
                    param.pid = _AppUser.ProjectId;
                    param.gid = _AppUser.GroupId;
                    param.uid = _AppUser.Id;
                    break;
            }

            ViewBag.PagerOption = pageOption;

            // 查询到的数据
            int totalCount = APIInvoker.Post<int>(_AppConfig.WebApiHost + "api/Resource/GetResourceCountByCondition", param);
            pageOption.TotalCount = totalCount;

            string searchUrl = _AppConfig.WebApiHost + "api/Resource/GetResourcesByCondition";
            List<Models.Resource.Resource> resources = APIInvoker.Post<List<Models.Resource.Resource>>(searchUrl, param);
            ResourceDataFormat(resources, viewModel.StatusList, viewModel.SourceList, viewModel.InclinationList);
            viewModel.Resources = resources;

            return View(viewModel);
        }

        // 公共资源
        public IActionResult Public(ResourceSearchParams param, int id = 1)
        {
            Models.Resource.ResourceListViewModel viewModel = new ResourceListViewModel();

            viewModel.Params = param;

            viewModel.ProjectList = AppDTO.GetProjectsFromDb(_AppUser.OrgId);
            viewModel.StatusList = AppDTO.GetStatusFromJson();
            viewModel.InclinationList = AppDTO.GetInclinationsFromJson();
            viewModel.SourceList = AppDTO.GetSourcesFromDb(_AppUser.OrgId);
            viewModel.TalkCountList = AppDTO.GetTalkCountFromJson();
            viewModel.ProjectId = _AppUser.ProjectId;

            if (param.pi <= 0) param.pi = 1;
            param.ps = _AppConfig.PageSize;
            param.status = 1;

            // 分页参数
            var pageOption = new PagerOption {
                PageIndex = param.pi,
                PageSize = param.ps,
                TotalCount = 0,
                RouteUrl = "/Resource/List",
                QueryString = Request.QueryString.Value
            };

            #region 处理默认值项目(ProjectId，该值为必选)
            if (string.IsNullOrEmpty(Request.QueryString.Value)) {
                if (viewModel.ProjectId == null || viewModel.ProjectId <= 0) {
                    if (viewModel.ProjectList != null && viewModel.ProjectList.Count > 0) {
                        pageOption.QueryString = "?pid=" + viewModel.ProjectList.FirstOrDefault().Id;
                        param.pid = viewModel.ProjectList.FirstOrDefault().Id;
                    }
                }
                else {
                    pageOption.QueryString = "?pid=" + viewModel.ProjectId;
                    param.pid = viewModel.ProjectId;
                }
            }
            #endregion

            #region 如果查询参数中已有groupId和salerId，则为其加载下拉列表数据
            if (param.pid != null && param.pid > 0) {
                string apiGroup = _AppConfig.WebApiHost + "api/Group/GetGroupsByProjectId?projectId=" + param.pid;
                viewModel.GroupList = APIInvoker.Get<List<Models.Group.Group>>(apiGroup);
                if (param.gid != null && param.gid > 0) {
                    string apiUser = _AppConfig.WebApiHost + "api/AppUser/GetAllUsersByGroupId?groupId=" + param.gid;
                    viewModel.SalerList = APIInvoker.Get<List<Models.AppUser.AppUserViewModel>>(apiUser);
                }
            }
            #endregion

            ViewBag.PagerOption = pageOption;

            // 查询到的数据
            int totalCount = APIInvoker.Post<int>(_AppConfig.WebApiHost + "api/Resource/GetResourceCountByCondition", param);
            pageOption.TotalCount = totalCount;

            string searchUrl = _AppConfig.WebApiHost + "api/Resource/GetResourcesByCondition";
            List<Models.Resource.Resource> resources = APIInvoker.Post<List<Models.Resource.Resource>>(searchUrl, param);
            ResourceDataFormat(resources, viewModel.StatusList, viewModel.SourceList, viewModel.InclinationList);
            viewModel.Resources = resources;

            return View(viewModel);
        }

        // 无意向资源
        public IActionResult Unvalued(ResourceSearchParams param, int id = 1)
        {
            Models.Resource.ResourceListViewModel viewModel = new ResourceListViewModel();

            viewModel.Params = param;

            viewModel.ProjectList = AppDTO.GetProjectsFromDb(_AppUser.OrgId);
            viewModel.StatusList = AppDTO.GetStatusFromJson();
            viewModel.InclinationList = AppDTO.GetInclinationsFromJson();
            viewModel.SourceList = AppDTO.GetSourcesFromDb(_AppUser.OrgId);
            viewModel.TalkCountList = AppDTO.GetTalkCountFromJson();
            viewModel.ProjectId = _AppUser.ProjectId;

            if (param.pi <= 0) param.pi = 1;
            param.ps = _AppConfig.PageSize;
            param.status = 2;

            // 分页参数
            var pageOption = new PagerOption {
                PageIndex = param.pi,
                PageSize = param.ps,
                TotalCount = 0,
                RouteUrl = "/Resource/List",
                QueryString = Request.QueryString.Value
            };

            #region 处理默认值项目(ProjectId，该值为必选)
            if (string.IsNullOrEmpty(Request.QueryString.Value)) {
                if (viewModel.ProjectId == null || viewModel.ProjectId <= 0) {
                    if (viewModel.ProjectList != null && viewModel.ProjectList.Count > 0) {
                        pageOption.QueryString = "?pid=" + viewModel.ProjectList.FirstOrDefault().Id;
                        param.pid = viewModel.ProjectList.FirstOrDefault().Id;
                    }
                }
                else {
                    pageOption.QueryString = "?pid=" + viewModel.ProjectId;
                    param.pid = viewModel.ProjectId;
                }
            }
            #endregion

            #region 如果查询参数中已有groupId和salerId，则为其加载下拉列表数据
            if (param.pid != null && param.pid > 0) {
                string apiGroup = _AppConfig.WebApiHost + "api/Group/GetGroupsByProjectId?projectId=" + param.pid;
                viewModel.GroupList = APIInvoker.Get<List<Models.Group.Group>>(apiGroup);
                if (param.gid != null && param.gid > 0) {
                    string apiUser = _AppConfig.WebApiHost + "api/AppUser/GetAllUsersByGroupId?groupId=" + param.gid;
                    viewModel.SalerList = APIInvoker.Get<List<Models.AppUser.AppUserViewModel>>(apiUser);
                }
            }
            #endregion

            ViewBag.PagerOption = pageOption;

            // 查询到的数据
            int totalCount = APIInvoker.Post<int>(_AppConfig.WebApiHost + "api/Resource/GetResourceCountByCondition", param);
            pageOption.TotalCount = totalCount;

            string searchUrl = _AppConfig.WebApiHost + "api/Resource/GetResourcesByCondition";
            List<Models.Resource.Resource> resources = APIInvoker.Post<List<Models.Resource.Resource>>(searchUrl, param);
            ResourceDataFormat(resources, viewModel.StatusList, viewModel.SourceList, viewModel.InclinationList);
            viewModel.Resources = resources;

            return View(viewModel);
        }

        // 资源录入
        public IActionResult Add()
        {
            Models.Resource.ResourceViewModel viewModel = new Models.Resource.ResourceViewModel();
            
            // 当前组织/公司下的项目集合
            viewModel.ProjectId = Convert.ToInt32(_AppUser.ProjectId);
            viewModel.Projects = AppDTO.GetProjectsFromDb(_AppUser.OrgId);

            // 意向群
            viewModel.Inclinations = AppDTO.GetInclinationsFromJson();
            // 资源来源
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
                    // 插入资源和组织机构之间的关系
                    int resourceOrgId = InsertResourceOrganization(resourceId, _AppUser.OrgId);
                    // 插入资源和项目之间的关系
                    int relationId = InsertResourceProject(resourceId, viewModel.ProjectId);
                    // 资源归属
                    if (viewModel.ResourceBelong == 1 && _AppUser.GroupId != null && _AppUser.GroupId > 0) {
                        // 划给自己
                        int resourceGroupId = InsertResourceGroup(resourceId, (int)_AppUser.GroupId);
                        int resourceUserId = InsertResourceUser(resourceId, _AppUser.Id); 
                    }
                    else {
                        // 划入'未分配'
                        bool res = SetResourceStatus(resourceId, 3);
                    }
                    result = true;
                }
                
                TempData["result"] = result;
                return RedirectToAction("Add");
            }
            return View();
        }

        // 资源批量导入
        public IActionResult BatchImport()
        {
            return View();
        }

        // 批量分配
        public IActionResult Assign()
        {
            ViewBag.ProjectList = AppDTO.GetProjectsFromDb(_AppUser.OrgId);
            ViewBag.ProjectId = _AppUser.ProjectId;
            if (_AppUser.ProjectId != null && _AppUser.ProjectId > 0) {
                ViewBag.GroupList = AppDTO.GetGroupsFromDb((int)_AppUser.ProjectId);
            }
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
                    // 获取尚未分配的资源
                    string unAssignedResourcesApi = _AppConfig.WebApiHost + $"api/Resource/GetGroupUnAssignedResources?projectId={projectId}";
                    List<UnassignedResource> resources = APIInvoker.Get<List<UnassignedResource>>(unAssignedResourcesApi);

                    List<ResourceGroup> resourceGroups = new List<ResourceGroup>();//分配关系ResourceGroup集合
                    // 开始分配
                    int i = 0;
                    foreach (var item in assignInfoList) {
                        if (item.assignCount > 0) {
                            int spliceCount = Math.Min(i + item.assignCount, resources.Count);
                            var blocks =  resources.GetRange(i, spliceCount);//切出的数据块
                            i += spliceCount;
                            // 生成分配关系ResourceGroup
                            if(blocks != null && blocks.Count > 0) {
                                DateTime now = DateTime.Now;
                                foreach(var resource in blocks) {
                                    resourceGroups.Add(new ResourceGroup() { ResourceId = resource.ResourceId, GroupId = item.groupId, CreateTime = now });
                                }
                            }
                        }
                    }
                    // 插入数据库
                    foreach(var item in resourceGroups) {
                        InsertResourceGroup(item.ResourceId, item.GroupId);
                    }
                }
                TempData["result"] = true;
                return RedirectToAction("Assign");
            }
            return View();
        }

        // 资源来源统计
        public IActionResult SourceStat()
        {
            string apiUrl = _AppConfig.WebApiHost + "api/Resource/GetResourceSourceStat?orgId=" + _AppUser.OrgId;
            var list = APIInvoker.Get<List<Models.Resource.ResourceSource>>(apiUrl);
            ViewBag.SourceStat = JsonConvert.SerializeObject(list);

            return View();
        }

        /// <summary>
        /// 资源/客户详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Detail(int id)
        {
            ResourceDetailViewModel viewModel = new ResourceDetailViewModel();

            string resourceApiUrl = _AppConfig.WebApiHost + "api/Resource/GetResourceById?id=" + id;
            Models.Resource.Resource resource = APIInvoker.Get<Models.Resource.Resource>(resourceApiUrl);

            if (resource != null) { 
                // 客户(资源)信息
                viewModel.ResourceId = id;
                viewModel.CustomerName = resource.CustomerName;
                viewModel.Mobile = resource.Mobile;
                viewModel.QQ = resource.QQ;
                viewModel.Wechat = resource.Wechat;
                viewModel.Tel = resource.Tel;
                viewModel.Email = resource.Email;
                viewModel.SourceFrom = resource.SourceFrom;
                viewModel.Status = resource.Status;
                viewModel.Inclination = resource.Inclination;
                viewModel.Sex = resource.Sex;
                viewModel.Address = resource.Address;
                viewModel.Remark = resource.Remark;
                // 便签
                string apiNote = _AppConfig.WebApiHost + "api/ResourceNote/GetNotesByResourceId?resourceId=" + id;
                viewModel.ResourceNotes = APIInvoker.Get<List<Models.Resource.ResourceNote>>(apiNote);

                // 洽谈记录
                string apiRecord = _AppConfig.WebApiHost + "api/TalkRecord/GetRecordsByResourceId?resourceId=" + id;
                viewModel.TalkRecords = APIInvoker.Get<List<Models.Resource.TalkRecord>>(apiRecord);

                // 签约记录
                string apiSign = _AppConfig.WebApiHost + "api/CustomerSign/GetSignByResourceId?resourceId=" + id;
                viewModel.Sign = APIInvoker.Get<Models.Sign.CustomerSign>(apiSign);

                // 资源状态&意向&来源
                viewModel.StatusList = AppDTO.GetStatusFromJson();
                viewModel.InclinationList = AppDTO.GetInclinationsFromJson();
                viewModel.SourceList = AppDTO.GetSourcesFromDb(_AppUser.OrgId);
  
            }
            else {
                return RedirectToAction("Http404", "Error");
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult DetailHandler(Models.Resource.ResourceDetailViewModel viewModel)
        {
            if (viewModel != null && viewModel.ResourceId > 0) {
                string resourceApiUrl = _AppConfig.WebApiHost + "api/Resource/GetResourceById?id=" + viewModel.ResourceId;
                Models.Resource.Resource resource = APIInvoker.Get<Models.Resource.Resource>(resourceApiUrl);
                if (resource != null) {
                    resource.CustomerName = viewModel.CustomerName;
                    resource.Mobile = viewModel.Mobile;
                    resource.Wechat = viewModel.Wechat;
                    resource.QQ = viewModel.QQ;
                    resource.Tel = viewModel.Tel;
                    resource.SourceFrom = viewModel.SourceFrom;
                    resource.Sex = viewModel.Sex;
                    resource.Inclination = viewModel.Inclination;
                    resource.Address = viewModel.Address;
                    resource.Status = viewModel.Status;
                    resource.Remark = viewModel.Remark;
                    resource.UpdateTime = DateTime.Now;

                    string resourceUpdateApiUrl = _AppConfig.WebApiHost + "api/Resource/UpdateReource";
                    bool result = APIInvoker.Post<bool>(resourceUpdateApiUrl, resource);
                    TempData["result"] = result;

                    return RedirectToAction("Detail", new { id = viewModel.ResourceId });
                }
            }
            return RedirectToAction("Index", "Error");
        }

        // 客户查询工具
        public IActionResult Search(CustomerSearchViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.key)) return View(viewModel);

            string resourceApiUrl = _AppConfig.WebApiHost + "api/Resource/GetResourceByNameMobileWechatQQ?key=" + viewModel.key + "&orgId=" + _AppUser.OrgId;
            Models.Resource.Resource resource = APIInvoker.Get<Models.Resource.Resource>(resourceApiUrl);

            if (resource != null) {
                // 客户(资源)信息
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

                // 洽谈记录
                string apiRecord = _AppConfig.WebApiHost + "api/TalkRecord/GetRecordsByResourceId?resourceId=" + resource.Id;
                viewModel.TalkRecords = APIInvoker.Get<List<Models.Resource.TalkRecord>>(apiRecord);

                // 签约记录
                string apiSign = _AppConfig.WebApiHost + "api/CustomerSign/GetSignByResourceId?resourceId=" + resource.Id;
                viewModel.Sign = APIInvoker.Get<Models.Sign.CustomerSign>(apiSign);

                // 组织机构下的业务员
                string apiUser = _AppConfig.WebApiHost + "api/AppUser/GetAllUsersByProjectId?projectId=" + resource.ProjectId;
                viewModel.OrgUsers = APIInvoker.Get<List<Models.AppUser.AppUserComplex>>(apiUser);
            }

            return View(viewModel);
        }

        // 插入客户签约记录
        public bool InsertCustomerSign(Models.Sign.CustomerSign sign)
        {
            if (sign != null && sign.Amount > 0) {
                sign.AppendUserId = _AppUser.Id;
                sign.CreateTime = DateTime.Now;
                string signApiUrl = _AppConfig.WebApiHost + "api/CustomerSign/InsertSign";
                bool result = APIInvoker.Post<bool>(signApiUrl, sign);

                if (result) {
                    // 更新当前资源状态为“已签约”
                    string statusApi = _AppConfig.WebApiHost + $"api/Resource/SetResourceStatus?resourceId={sign.ResourceId}&status=5";
                    result = APIInvoker.Get<bool>(statusApi);
                }

                return result;
            }
            return false;
        }

        //[HttpPost]
        //public IActionResult SearchHandler()
        //{
        //    return View();
        //}

        #region 插入资源 InsertResource
        private int InsertResource(Models.Resource.ResourceViewModel viewModel)
        {
            var resource = new
            {
                CustomerName = viewModel.CustomerName,
                Message = viewModel.Message,
                Mobile = viewModel.Mobile,
                Tel = viewModel.Tel,
                Wechat = viewModel.Wechat,
                QQ = viewModel.QQ,
                Email = viewModel.Email,
                Address = viewModel.Address,
                Inclination = viewModel.Inclination,
                TalkCount = 0,
                SourceFrom = viewModel.SourceFrom,
                Status = 3,
                AppendUserId = _AppUser.Id,
                Remark = viewModel.Remark,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                DeleteFlag = 0
            };

            string apiUrl = _AppConfig.WebApiHost + "api/Resource/InsertResource";
            int identityId = APIInvoker.Post<int>(apiUrl, resource);

            return identityId;
        }
        #endregion

        #region 插入资源和组织机构之间的关系
        private int InsertResourceOrganization(int resourceId, int orgId)
        {
            var resourceOrg = new
            {
                ResourceId = resourceId,
                OrgId = orgId,
                CreateTime = DateTime.Now
            };

            string apiUrl = _AppConfig.WebApiHost + "api/Resource/InsertResourceOrganization";
            int identityId = APIInvoker.Post<int>(apiUrl, resourceOrg);

            return identityId;
        }
        #endregion

        #region 插入资源和项目的关系
        private int InsertResourceProject(int resourceId, int projectId)
        {
            var resourceProject = new
            {
                ResourceId = resourceId,
                ProjectId = projectId,
                CreateTime = DateTime.Now
            };

            string apiUrl = _AppConfig.WebApiHost + "api/Resource/InsertResourceProject";
            int identityId = APIInvoker.Post<int>(apiUrl, resourceProject);

            return identityId;
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

        #region 插入资源和用户的关系
        private int InsertResourceUser(int resourceId, int userId)
        {
            var resourceUser = new
            {
                ResourceId = resourceId,
                UserId = userId,
                CreateTime = DateTime.Now
            };

            string apiUrl = _AppConfig.WebApiHost + "api/Resource/InsertResourceUser";
            int identityId = APIInvoker.Post<int>(apiUrl, resourceUser);

            return identityId;
        }
        #endregion

        #region 资源列表数据的加工处理
        /// <summary>
        /// 资源数据的加工处理
        /// </summary>
        /// <param name="resources"></param>
        /// <param name="status"></param>
        /// <param name="sources"></param>
        /// <param name="inclinations"></param>
        public void ResourceDataFormat(List<Models.Resource.Resource> resources, List<SelectItem> status, List<Models.Source.Source> sources, List<SelectItem> inclinations)
        {
            if (resources != null) {
                foreach (var resource in resources) {
                    // 联系方式ContactInfo的处理
                    string contactInfo = "";
                    if (!string.IsNullOrEmpty(resource.Mobile)) {
                        contactInfo = $"[手:{resource.Mobile}]";
                    }
                    else if (!string.IsNullOrEmpty(resource.Tel)) {
                        contactInfo = $"[固:{resource.Tel}]";
                    }
                    else if (!string.IsNullOrEmpty(resource.QQ)) {
                        contactInfo = $"[QQ:{resource.QQ}]";
                    }
                    else if (!string.IsNullOrEmpty(resource.Wechat)) {
                        contactInfo = $"[微信:{resource.Wechat}]";
                    }
                    else if (!string.IsNullOrEmpty(resource.Email)) {
                        contactInfo = $"[Email:{resource.Email}]";
                    }
                    resource.ContactInfo = contactInfo;
                    // 来源
                    if (resource.SourceFrom != null && sources != null) {
                        var query = sources.FirstOrDefault(x => x.Id == resource.SourceFrom);
                        if (query != null) {
                            resource.SourceFromText = query.SourceName;
                        }
                    }
                    // 意向
                    if (resource.Inclination != null && inclinations != null) {
                        var query = inclinations.FirstOrDefault(x => x.value == resource.Inclination);
                        if (query != null) {
                            resource.InclinationText = query.displayText;
                        }
                    }
                    // 状态
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

        #region 单个资源分配 ResourceAssign
        /// <summary>
        /// 资源分配
        /// 资源和业务组，资源和用户都是一对一关系
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
            // 插入ResourceGroup表
            string rgGetAPI = _AppConfig.WebApiHost + "api/Resource/GetResourceGroup?resourceId=" + resourceId;
            string rgUpdateAPI = _AppConfig.WebApiHost + "api/Resource/UpdateResourceGroup";
            string rgInsertAPI = _AppConfig.WebApiHost + "api/Resource/InsertResourceGroup";

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

            // 插入ResourceUser表
            string ruGetAPI = _AppConfig.WebApiHost + "api/Resource/GetResourceUser?resourceId=" + resourceId;
            string ruUpdateAPI = _AppConfig.WebApiHost + "api/Resource/UpdateResourceUser";
            string ruInsertAPI = _AppConfig.WebApiHost + "api/Resource/InsertResourceUser";
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

            return assignResult;
        }
        #endregion

        #region 设置资源状态
        private bool SetResourceStatus(int resourceId, int status)
        {
            string apiUrl = _AppConfig.WebApiHost + $"api/Resource/SetResourceStatus?resourceId={resourceId}&status={status}";

            bool result = APIInvoker.Get<bool>(apiUrl);
            return result;
        } 
        #endregion

        #region 删除资源
        /// <summary>
        /// 删除资源 ajax
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public bool DeleteResource(int id)
        {
            string apiUrl = _AppConfig.WebApiHost + "api/Resource/DeleteResource?id=" + id;
            bool result = APIInvoker.Get<bool>(apiUrl);
            return result;
        }
        #endregion

        #region 资源是否存在
        public bool IsExist(string mobile, string tel, string qq, string wechat)
        {
            string apiUrl = _AppConfig.WebApiHost + $"api/Resource/IsResourceExist?orgId={_AppUser.OrgId}&mobile={mobile}&tel={tel}&qq={qq}&wechat={wechat}";
            bool result = APIInvoker.Get<bool>(apiUrl);
            return result;
        } 
        #endregion

        #region 添加一条洽谈记录
        [HttpPost]
        public bool AddTalkRecord(int resourceId, int talkWay, string talkResult)
        {
            string apiUrl = _AppConfig.WebApiHost + "api/TalkRecord/InsertTalkRecord";
            var record = new
            {
                ResourceId = resourceId,
                TalkWay = talkWay,
                TalkResult = talkResult,
                UserId = _AppUser.Id,
                CreateTime = DateTime.Now
            };

            bool result = APIInvoker.Post<bool>(apiUrl, record);
            return result;
        } 
        #endregion

        /// <summary>
        /// 获取角色下的数据权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        private List<Models.Role.RoleDataPermission> GetRoleDataPermissions(int roleId)
        {
            string apiUrl = _AppConfig.WebApiHost + "api/DataPermission/GetRoleDataPermissions?roleId=" + roleId;
            var dataResult = APIInvoker.Get<List<Models.Role.RoleDataPermission>>(apiUrl);
            return dataResult;
        }

        /// <summary>
        /// 角色的数据权限：获取资源可见范围
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
        /// 角色的数据权限：获取资源操作权限
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
        /// 获取未分配至业务组的资源个数
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public int GetGroupUnAssignedResourceCount(int projectId)
        {
            string apiUrl = _AppConfig.WebApiHost + "api/Resource/GetGroupUnAssignedResourceCount?projectId=" + projectId;
            int count = APIInvoker.Get<int>(apiUrl);
            return count;
        }

        /// <summary>
        /// 获取未分配至业务组的资源个数
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public int GetUserUnAssignedResourceCount()
        {
            string apiUrl = _AppConfig.WebApiHost + "api/Resource/GetUserUnAssignedResourceCount?orgId=" + _AppUser.OrgId;
            int count = APIInvoker.Get<int>(apiUrl);
            return count;
        }
    }
}