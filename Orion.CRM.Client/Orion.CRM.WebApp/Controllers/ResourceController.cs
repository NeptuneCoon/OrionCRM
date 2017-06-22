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
    /// ��Դ���������
    /// </summary>
    public class ResourceController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnv;
        public ResourceController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnv = hostingEnvironment;
        }

        // ��Դ�б�
        public IActionResult List(ResourceSearchParams param)
        {
            Models.Resource.ResourceListViewModel viewModel = new ResourceListViewModel();
            viewModel.Params = param;

            viewModel.ProjectList = AppDTO.GetProjectsFromDb(_AppConfig.WebAPIHost, _AppUser.OrgId);
            viewModel.StatusList = AppDTO.GetStatusFromJson(_hostingEnv.WebRootPath);
            viewModel.InclinationList = AppDTO.GetInclinationsFromJson(_hostingEnv.WebRootPath);
            viewModel.SourceList = AppDTO.GetSourcesFromDb(_AppConfig.WebAPIHost, _AppUser.OrgId);
            viewModel.TalkCountList = AppDTO.GetTalkCountFromJson(_hostingEnv.WebRootPath);
            viewModel.ProjectId = _AppUser.ProjectId;

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

            #region ����Ĭ��ֵ��Ŀ(ProjectId����ֵΪ��ѡ)
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

            ViewBag.PagerOption = pageOption;

            // ��ѯ��������
            int totalCount = APIInvoker.Post<int>(_AppConfig.WebAPIHost + "api/Resource/GetResourceCountByCondition", param);
            pageOption.TotalCount = totalCount;

            string searchUrl = _AppConfig.WebAPIHost + "api/Resource/GetResourcesByCondition";
            List<Models.Resource.Resource> resources = APIInvoker.Post<List<Models.Resource.Resource>>(searchUrl, param);
            ResourceDataFormat(resources, viewModel.StatusList, viewModel.SourceList, viewModel.InclinationList);
            viewModel.Resources = resources;

            return View(viewModel);
        }

        // ������Դ
        public IActionResult Public(ResourceSearchParams param, int id = 1)
        {
            Models.Resource.ResourceListViewModel viewModel = new ResourceListViewModel();

            viewModel.Params = param;

            viewModel.ProjectList = AppDTO.GetProjectsFromDb(_AppConfig.WebAPIHost, _AppUser.OrgId);
            viewModel.StatusList = AppDTO.GetStatusFromJson(_hostingEnv.WebRootPath);
            viewModel.InclinationList = AppDTO.GetInclinationsFromJson(_hostingEnv.WebRootPath);
            viewModel.SourceList = AppDTO.GetSourcesFromDb(_AppConfig.WebAPIHost, _AppUser.OrgId);
            viewModel.TalkCountList = AppDTO.GetTalkCountFromJson(_hostingEnv.WebRootPath);
            viewModel.ProjectId = _AppUser.ProjectId;

            if (param.pi <= 0) param.pi = 1;
            param.ps = _AppConfig.PageSize;
            param.status = 1;

            // ��ҳ����
            var pageOption = new PagerOption {
                PageIndex = param.pi,
                PageSize = param.ps,
                TotalCount = 0,
                RouteUrl = "/Resource/List",
                QueryString = Request.QueryString.Value
            };

            #region ����Ĭ��ֵ��Ŀ(ProjectId����ֵΪ��ѡ)
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

            ViewBag.PagerOption = pageOption;

            // ��ѯ��������
            int totalCount = APIInvoker.Post<int>(_AppConfig.WebAPIHost + "api/Resource/GetResourceCountByCondition", param);
            pageOption.TotalCount = totalCount;

            string searchUrl = _AppConfig.WebAPIHost + "api/Resource/GetResourcesByCondition";
            List<Models.Resource.Resource> resources = APIInvoker.Post<List<Models.Resource.Resource>>(searchUrl, param);
            ResourceDataFormat(resources, viewModel.StatusList, viewModel.SourceList, viewModel.InclinationList);
            viewModel.Resources = resources;

            return View(viewModel);
        }

        // ��������Դ
        public IActionResult Unvalued(ResourceSearchParams param, int id = 1)
        {
            Models.Resource.ResourceListViewModel viewModel = new ResourceListViewModel();

            viewModel.Params = param;

            viewModel.ProjectList = AppDTO.GetProjectsFromDb(_AppConfig.WebAPIHost, _AppUser.OrgId);
            viewModel.StatusList = AppDTO.GetStatusFromJson(_hostingEnv.WebRootPath);
            viewModel.InclinationList = AppDTO.GetInclinationsFromJson(_hostingEnv.WebRootPath);
            viewModel.SourceList = AppDTO.GetSourcesFromDb(_AppConfig.WebAPIHost, _AppUser.OrgId);
            viewModel.TalkCountList = AppDTO.GetTalkCountFromJson(_hostingEnv.WebRootPath);
            viewModel.ProjectId = _AppUser.ProjectId;

            if (param.pi <= 0) param.pi = 1;
            param.ps = _AppConfig.PageSize;
            param.status = 2;

            // ��ҳ����
            var pageOption = new PagerOption {
                PageIndex = param.pi,
                PageSize = param.ps,
                TotalCount = 0,
                RouteUrl = "/Resource/List",
                QueryString = Request.QueryString.Value
            };

            #region ����Ĭ��ֵ��Ŀ(ProjectId����ֵΪ��ѡ)
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

            ViewBag.PagerOption = pageOption;

            // ��ѯ��������
            int totalCount = APIInvoker.Post<int>(_AppConfig.WebAPIHost + "api/Resource/GetResourceCountByCondition", param);
            pageOption.TotalCount = totalCount;

            string searchUrl = _AppConfig.WebAPIHost + "api/Resource/GetResourcesByCondition";
            List<Models.Resource.Resource> resources = APIInvoker.Post<List<Models.Resource.Resource>>(searchUrl, param);
            ResourceDataFormat(resources, viewModel.StatusList, viewModel.SourceList, viewModel.InclinationList);
            viewModel.Resources = resources;

            return View(viewModel);
        }

        // ��Դ¼��
        public IActionResult Add()
        {
            Models.Resource.ResourceViewModel viewModel = new Models.Resource.ResourceViewModel();
            
            // ��ǰ��֯/��˾�µ���Ŀ����
            viewModel.ProjectId = Convert.ToInt32(_AppUser.ProjectId);
            viewModel.Projects = AppDTO.GetProjectsFromDb(_AppConfig.WebAPIHost, _AppUser.OrgId);

            // ����Ⱥ
            viewModel.Inclinations = AppDTO.GetInclinationsFromJson(_hostingEnv.WebRootPath);

            // ��Դ��Դ
            viewModel.Sources = AppDTO.GetSourcesFromDb(_AppConfig.WebAPIHost, _AppUser.OrgId);

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddHandler(Models.Resource.ResourceViewModel viewModel)
        {
            if (viewModel != null) {
                bool result = false;

                int resourceId = InsertResource(viewModel);
                if (resourceId > 0) {
                    // ������Դ����Ŀ֮��Ĺ�ϵ
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

        // ��Դ��������
        public IActionResult BatchImport()
        {
            return View();
        }

        // ��������
        public IActionResult Assign()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AssignHandler()
        {
            return View();
        }

        // ��Դ��Դͳ��
        public IActionResult SourceStat()
        {
            return View();
        }

        #region ������Դ
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

            string apiUrl = _AppConfig.WebAPIHost + "api/Resource/InsertResource";
            int identityId = APIInvoker.Post<int>(apiUrl, resource);

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

            string apiUrl = _AppConfig.WebAPIHost + "api/Resource/InsertResourceProject";
            int identityId = APIInvoker.Post<int>(apiUrl, resourceProject);

            return identityId;
        }
        #endregion

        #region ��Դ���ݵļӹ�����
        /// <summary>
        /// ��Դ���ݵļӹ�����
        /// </summary>
        /// <param name="resources"></param>
        /// <param name="status"></param>
        /// <param name="sources"></param>
        /// <param name="inclinations"></param>
        public void ResourceDataFormat(List<Models.Resource.Resource> resources, List<SelectItem> status, List<Models.Source.ResourceSource> sources, List<SelectItem> inclinations)
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

        #region ��Դ����
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
            // ����ResourceGroup��
            string rgGetAPI = _AppConfig.WebAPIHost + "api/Resource/GetResourceGroup?resourceId=" + resourceId;
            string rgUpdateAPI = _AppConfig.WebAPIHost + "api/Resource/UpdateResourceGroup";
            string rgInsertAPI = _AppConfig.WebAPIHost + "api/Resource/InsertResourceGroup";

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
            string ruGetAPI = _AppConfig.WebAPIHost + "api/Resource/GetResourceUser?resourceId=" + resourceId;
            string ruUpdateAPI = _AppConfig.WebAPIHost + "api/Resource/UpdateResourceUser";
            string ruInsertAPI = _AppConfig.WebAPIHost + "api/Resource/InsertResourceUser";
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
    }
}