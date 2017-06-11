using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Orion.CRM.WebTools;
using Newtonsoft.Json;

namespace Orion.CRM.ConsoleApp.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly AppConfig _appConfig;
        public OrganizationController(IOptions<AppConfig> optionsAccessor)
        {
            _appConfig = optionsAccessor.Value;
        }

        public IActionResult OrgList()
        {
            string url = _appConfig.WebAPIHost + "api/Organization/GetAllOrganizations";
            List<Models.Organization.OrganizationViewModel> list = APIInvoker.Get<List<Models.Organization.OrganizationViewModel>>(url);

            ViewBag.OperateResult = Request.Query["operateResult"].ToString();
            return View(list);
        }

        public IActionResult Create()
        {
            Models.Organization.OrganizationViewModel viewModel = new Models.Organization.OrganizationViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CreateHandler(Models.Organization.OrganizationViewModel viewModel)
        {
            if (viewModel != null) {
                string apiUrl = _appConfig.WebAPIHost + "api/Organization/CreateOrganization";
                var org = new {
                    OrgName = viewModel.OrgName,
                    OrgCode = viewModel.OrgCode,
                    Type = 1,
                    CreateTime = DateTime.Now,
                    DeleteFlag = 0
                };

                int primaryId = APIInvoker.Post<int>(apiUrl, org);

                if (primaryId > 0) {
                    return RedirectToAction("OrgList", new { operateResult = "success" });
                }
                else {
                    return RedirectToAction("OrgList", new { operateResult = "fail" });
                }
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            Models.Organization.OrganizationViewModel viewModel = new Models.Organization.OrganizationViewModel();

            string url = _appConfig.WebAPIHost + "api/Organization/GetOrganizationById?id=" + id;
            viewModel = APIInvoker.Get<Models.Organization.OrganizationViewModel>(url);

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult EditHandler(Models.Organization.OrganizationViewModel viewModel)
        {
            if (viewModel != null) {
                string apiUrl = _appConfig.WebAPIHost + "api/Organization/UpdateOrganization";
                var org = new {
                    Id = viewModel.Id,
                    OrgName = viewModel.OrgName,
                    OrgCode = viewModel.OrgCode,
                    Type = 1,
                    DeleteFlag = 0
                };
                bool result = APIInvoker.Post<bool>(apiUrl, org);

                if (result) {
                    return RedirectToAction("OrgList", new { operateResult = "success" });
                }
                else {
                    return RedirectToAction("OrgList", new { operateResult = "fail" });
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
            string getURL = _appConfig.WebAPIHost + "api/Organization/GetOrganizationById?id=" + id;
            Models.Organization.OrganizationViewModel org = APIInvoker.Get<Models.Organization.OrganizationViewModel>(getURL);
            org.DeleteFlag = true;

            // 更新
            string updateURL = _appConfig.WebAPIHost + "api/Organization/UpdateOrganization";
            bool result = APIInvoker.Post<bool>(updateURL, org);

            return result;
        }
    }
}