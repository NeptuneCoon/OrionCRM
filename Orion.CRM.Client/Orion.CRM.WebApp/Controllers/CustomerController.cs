using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orion.CRM.WebApp.App_Data;
using Orion.CRM.WebTools;

namespace Orion.CRM.WebApp.Controllers
{
    public class CustomerController : BaseController
    {
        public IActionResult All(Models.Customer.CustomerSearchParams param)
        {
            if (param.pi <= 0) param.pi = 1;
            param.ps = _AppConfig.PageSize;

            // 分页参数
            var pageOption = new PagerOption
            {
                PageIndex = param.pi,
                PageSize = param.ps,
                TotalCount = 0,
                RouteUrl = "/Customer/All",
                QueryString = Request.QueryString.Value
            };
            ViewBag.PagerOption = pageOption;

            // 查询到的数据
            int totalCount = APIInvoker.Post<int>(_AppConfig.WebApiHost + "/api/Customer/GetCustomersCountByCondition", param);
            pageOption.TotalCount = totalCount;

            string searchUrl = _AppConfig.WebApiHost + "/api/Customer/GetCustomersByCondition";
            List<Models.Customer.CustomerModel> customers = APIInvoker.Post<List<Models.Customer.CustomerModel>>(searchUrl, param);

            Models.Customer.CustomerListViewModel viewModel = new Models.Customer.CustomerListViewModel();
            viewModel.Params = param;
            viewModel.Customers = customers;
            viewModel.ProjectList = AppDTO.GetProjectsFromDb(_AppUser.OrgId);


            // 组织机构下的业务员
            string apiUser = _AppConfig.WebApiHost + "/api/AppUser/GetUsersByOrgId?pageIndex=1&pageSize=2000&orgId=" + _AppUser.OrgId;
            viewModel.OrgUsers = APIInvoker.Get<List<Models.AppUser.AppUserComplex>>(apiUser);

            return View(viewModel);
        }

        public IActionResult My(Models.Customer.CustomerSearchParams param)
        {
            if (param.pi <= 0) param.pi = 1;
            param.ps = _AppConfig.PageSize;

            // 分页参数
            var pageOption = new PagerOption
            {
                PageIndex = param.pi,
                PageSize = param.ps,
                TotalCount = 0,
                RouteUrl = "/Customer/My",
                QueryString = Request.QueryString.Value
            };
            ViewBag.PagerOption = pageOption;

            // 查询到的数据
            param.suid = _AppUser.Id;
            int totalCount = APIInvoker.Post<int>(_AppConfig.WebApiHost + "/api/Customer/GetCustomersCountByCondition", param);
            pageOption.TotalCount = totalCount;

            string searchUrl = _AppConfig.WebApiHost + "/api/Customer/GetCustomersByCondition";
            List<Models.Customer.CustomerModel> customers = APIInvoker.Post<List<Models.Customer.CustomerModel>>(searchUrl, param);

            Models.Customer.CustomerListViewModel viewModel = new Models.Customer.CustomerListViewModel();
            viewModel.Params = param;
            viewModel.Customers = customers;
            viewModel.ProjectList = AppDTO.GetProjectsFromDb(_AppUser.OrgId);

            return View(viewModel);
        }

        public IActionResult Add()
        {
            ViewBag.Projects = AppDTO.GetProjectsFromDb(_AppUser.OrgId);

            Models.Customer.CustomerModel viewModel = new Models.Customer.CustomerModel();

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddHandler(Models.Customer.CustomerModel customer)
        {
            if (customer != null)
            {
                string url = _AppConfig.WebApiHost + "/api/Customer/InsertCustomer";
                int identityId = APIInvoker.Post<int>(url, customer);
                TempData["result"] = identityId > 0;
            }

            return RedirectToAction("All","Customer");
        }

        public IActionResult Detail()
        {
            return View();
        }

        /// <summary>
        /// ajax调用：批量为客户分配售后专员
        /// </summary>
        /// <param name="customerIds">客户</param>
        /// <param name="userId">售后专员UserId</param>
        /// <returns></returns>
        [HttpGet]
        public int AssignServiceUser(string customerIds, int userId)
        {
            string apiUrl = _AppConfig.WebApiHost + $"/api/Customer/AssignServiceUser?customerIds={customerIds}&userId={userId}";
            int count = APIInvoker.Get<int>(apiUrl);
            return count;
        }

    }
}