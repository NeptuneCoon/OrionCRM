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

namespace Orion.CRM.WebApp.Controllers
{
    /// <summary>
    /// 资源管理控制器
    /// </summary>
    public class ResourceController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public ResourceController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        // 资源列表
        public IActionResult List()
        {
            AppConfig _c = _AppConfig;
            return View();
        }

        // 公共资源
        public IActionResult Public()
        {
            return View();
        }

        // 无意向资源
        public IActionResult Unvalued()
        {
            return View();
        }

        // 资源录入
        public IActionResult Add()
        {
            Models.Resource.ResourceViewModel viewModel = new Models.Resource.ResourceViewModel();
            
            // 当前组织/公司下的项目集合
            viewModel.ProjectId = Convert.ToInt32(_AppUser.ProjectId);
            viewModel.Projects = AppDTO.GetProjectsFromDb(_AppConfig.WebAPIHost, _AppUser.OrgId);

            // 意向群
            viewModel.Inclinations = AppDTO.GetInclinationsFromJson(_hostingEnvironment.WebRootPath);

            // 资源来源
            //var apiUrl = _AppConfig.WebAPIHost + "api/ResourceSource/GetSourcesByOrgId?orgId=" + _AppUser.OrgId;
            //viewModel.Sources = APIInvoker.Get<List<Models.Source.ResourceSource>>(apiUrl);
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


        #region 获取项目名称
        //private string GetProjectNameById(int? projectId)
        //{
        //    if (projectId == null) return string.Empty;

        //    string apiUrl = _AppConfig.WebAPIHost + "api/Project/GetProjectById?id=" + projectId;
        //    Models.Project.ProjectViewModel project = APIInvoker.Get<Models.Project.ProjectViewModel>(apiUrl);
        //    if (project != null) {
        //        return project.ProjectName;
        //    }
        //    return string.Empty;
        //}
        #endregion

        #region 插入资源
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

        #region 插入资源和项目的关系
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
    }
}