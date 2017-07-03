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

            viewModel.ProjectList = AppDTO.GetProjectsFromDb(_AppConfig.WebApiHost, _AppUser.OrgId);
            viewModel.StatusList = AppDTO.GetStatusFromJson(_hostingEnv.WebRootPath);
            viewModel.InclinationList = AppDTO.GetInclinationsFromJson(_hostingEnv.WebRootPath);
            viewModel.SourceList = AppDTO.GetSourcesFromDb(_AppConfig.WebApiHost, _AppUser.OrgId);
            viewModel.TalkCountList = AppDTO.GetTalkCountFromJson(_hostingEnv.WebRootPath);
            viewModel.RolePermissions = this.GetRoleDataPermissions(_AppUser.RoleId);
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
            /*
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
            param.gid = _AppUser.GroupId;
            param.uid = _AppUser.Id;

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
            */


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

            viewModel.ProjectList = AppDTO.GetProjectsFromDb(_AppConfig.WebApiHost, _AppUser.OrgId);
            viewModel.StatusList = AppDTO.GetStatusFromJson(_hostingEnv.WebRootPath);
            viewModel.InclinationList = AppDTO.GetInclinationsFromJson(_hostingEnv.WebRootPath);
            viewModel.SourceList = AppDTO.GetSourcesFromDb(_AppConfig.WebApiHost, _AppUser.OrgId);
            viewModel.TalkCountList = AppDTO.GetTalkCountFromJson(_hostingEnv.WebRootPath);
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

            viewModel.ProjectList = AppDTO.GetProjectsFromDb(_AppConfig.WebApiHost, _AppUser.OrgId);
            viewModel.StatusList = AppDTO.GetStatusFromJson(_hostingEnv.WebRootPath);
            viewModel.InclinationList = AppDTO.GetInclinationsFromJson(_hostingEnv.WebRootPath);
            viewModel.SourceList = AppDTO.GetSourcesFromDb(_AppConfig.WebApiHost, _AppUser.OrgId);
            viewModel.TalkCountList = AppDTO.GetTalkCountFromJson(_hostingEnv.WebRootPath);
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
            viewModel.Projects = AppDTO.GetProjectsFromDb(_AppConfig.WebApiHost, _AppUser.OrgId);

            // 意向群
            viewModel.Inclinations = AppDTO.GetInclinationsFromJson(_hostingEnv.WebRootPath);

            // 资源来源
            viewModel.Sources = AppDTO.GetSourcesFromDb(_AppConfig.WebApiHost, _AppUser.OrgId);

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddHandler(Models.Resource.ResourceViewModel viewModel)
        {
            if (viewModel != null) {
                bool result = false;

                int resourceId = InsertResource(viewModel);
                if (resourceId > 0) {
                    // 插入资源和项目之间的关系
                    int relationId = InsertResourceProject(resourceId, viewModel.ProjectId);
                    if (relationId > 0) {
                        result = true;
                    }
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
            return View();
        }

        [HttpPost]
        public IActionResult AssignHandler()
        {
            return View();
        }

        // 资源来源统计
        public IActionResult SourceStat()
        {
            return View();
        }

        public IActionResult Detail(int id)
        {
            ResourceDetailViewModel viewModel = new ResourceDetailViewModel();

            string apiResource = _AppConfig.WebApiHost + "api/Resource/GetResourceById?id=" + id;
            Models.Resource.Resource resource = APIInvoker.Get<Models.Resource.Resource>(apiResource);

            if (resource != null) { 
                // 客户(资源)信息
                viewModel.ResourceId = id;
                viewModel.CustomerName = resource.CustomerName;
                viewModel.Mobile = resource.Mobile;
                viewModel.QQ = resource.QQ;
                viewModel.Wechat = resource.Wechat;
                viewModel.Email = resource.Email;
                viewModel.SourceFrom = "";
                viewModel.Sex = resource.Sex;
                viewModel.Address = resource.Address;
                viewModel.Remark = resource.Remark;
                // 便签
                string apiNote = _AppConfig.WebApiHost + "api/ResourceNote/GetNotesByResourceId?resourceId=" + id;
                viewModel.ResourceNotes = APIInvoker.Get<List<Models.Resource.ResourceNote>>(apiNote);

                // 洽谈记录
                string apiRecord = _AppConfig.WebApiHost + "api/TalkRecord/GetRecordsByResourceId?resourceId=" + id;
                viewModel.TalkRecords = APIInvoker.Get<List<Models.Resource.TalkRecord>>(apiRecord);
            }
            else {
                return RedirectToAction("Http404", "Error");
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult DetailHandler()
        {
            return View();
        }

        #region 插入资源 InsertResource
        private int InsertResource(Models.Resource.ResourceViewModel viewModel)
        {
            var resource = new
            {
                CustomerName = viewModel.CustomerName,
                Message = viewModel.Message,
                Mobile = viewModel.Mobile,
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

        #region 资源数据的加工处理
        /// <summary>
        /// 资源数据的加工处理
        /// </summary>
        /// <param name="resources"></param>
        /// <param name="status"></param>
        /// <param name="sources"></param>
        /// <param name="inclinations"></param>
        public void ResourceDataFormat(List<Models.Resource.Resource> resources, List<SelectItem> status, List<Models.Source.ResourceSource> sources, List<SelectItem> inclinations)
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

        #region 资源分配 ResourceAssign
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

        #region DeleteResource
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
    }
}