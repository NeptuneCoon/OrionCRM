using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orion.CRM.WebApp.App_Data;
using Microsoft.Extensions.Options;
using Orion.CRM.WebTools;
using Newtonsoft.Json;

namespace Orion.CRM.WebApp.Controllers
{
    /// <summary>
    /// 项目控制器
    /// </summary>
    public class ProjectController : BaseController
    {
  
        public IActionResult List()
        {
            string url = _AppConfig.WebAPIHost + "api/Project/GetProjectsByOrgId?orgId="+ _AppUser.OrgId;
            List<Models.Project.ProjectViewModel> list = APIInvoker.Get<List<Models.Project.ProjectViewModel>>(url);

            ViewBag.OperateResult = Request.Query["operateResult"].ToString();
            return View(list);
        }
        
        public IActionResult Create()
        {
            Models.Project.ProjectViewModel viewModel = new Models.Project.ProjectViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CreateHandler(Models.Project.ProjectViewModel viewModel)
        {
            if (viewModel != null) {
                string apiUrl = _AppConfig.WebAPIHost + "api/Project/InsertProject";
                var project = new
                {
                    OrgId = _AppUser.OrgId,
                    ProjectName = viewModel.ProjectName,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };

                int primaryId = APIInvoker.Post<int>(apiUrl, project);

                if (primaryId > 0) {
                    return RedirectToAction("List", new { operateResult = "success" });
                }
                else {
                    return RedirectToAction("List", new { operateResult = "fail" });
                }
            }
            return View();
        }
        /*
        public IActionResult Edit(int id)
        {
            Models.Project.ProjectViewModel viewModel = new Models.Project.ProjectViewModel();

            string url = _AppConfig.WebAPIHost + "api/Organization/GetOrganizationById?id=" + id;
            viewModel = APIInvoker.Get<Models.Project.ProjectViewModel>(url);

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult EditHandler(Models.Project.ProjectViewModel viewModel)
        {
            if (viewModel != null) {
                string apiUrl = _AppConfig.WebAPIHost + "api/Organization/UpdateOrganization";
                var org = new
                {
                    Id = viewModel.Id,
                    OrgName = viewModel.OrgName,
                    OrgCode = viewModel.OrgCode,
                    Type = 1,
                    DeleteFlag = 0
                };
                string requestData = JsonConvert.SerializeObject(org);
                bool result = APIInvoker.Post<bool>(apiUrl, requestData);

                if (result) {
                    return RedirectToAction("List", new { operateResult = "success" });
                }
                else {
                    return RedirectToAction("List", new { operateResult = "fail" });
                }
            }
            return View();
        }

        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteOrganization(int id)
        {
            // 获取
            string getURL = _AppConfig.WebAPIHost + "api/Organization/GetOrganizationById?id=" + id;
            Models.Project.ProjectViewModel org = APIInvoker.Get<Models.Project.ProjectViewModel>(getURL);
            org.DeleteFlag = true;

            // 更新
            string updateURL = _AppConfig.WebAPIHost + "api/Organization/UpdateOrganization";
            string requestData = JsonConvert.SerializeObject(org);
            bool result = APIInvoker.Post<bool>(updateURL, requestData);

            return result;
        }
        */
    }
}