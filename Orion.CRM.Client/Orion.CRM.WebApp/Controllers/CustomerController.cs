using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orion.CRM.WebApp.App_Data;
using Orion.CRM.WebTools;

namespace Orion.CRM.WebApp.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnv;
        public CustomerController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnv = hostingEnvironment;
        }
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

        public IActionResult Detail(int id)
        {
            Models.Customer.CustomerDetailViewModel viewModel = new Models.Customer.CustomerDetailViewModel();
            string apiUrl = _AppConfig.WebApiHost + "/api/Customer/GetCustomerById?id=" + id;
            Models.Customer.CustomerModel customer = APIInvoker.Get<Models.Customer.CustomerModel>(apiUrl);
            viewModel.Customer = customer;

            apiUrl = _AppConfig.WebApiHost + "/api/Customer/GetServiceRecordsByCustomerId?customerId=" + id;
            viewModel.ServiceRecords = APIInvoker.Get<List<Models.Customer.CustomerServiceRecord>>(apiUrl);

            return View(viewModel);
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

        // id=Customer表主键
        public IActionResult AddServiceRecord(int id)
        {
            string apiUrl = _AppConfig.WebApiHost + "/api/Customer/GetCustomerById?id=" + id;//id=customerId
            Models.Customer.CustomerModel customer = APIInvoker.Get<Models.Customer.CustomerModel>(apiUrl);

            return View(customer);
        }

        [HttpPost]
        public IActionResult AddServiceRecordHandler()
        {
            int customerId = Convert.ToInt32(Request.Form["CustomerId"]);
            string content = Request.Form["ServiceContent"]; // 服务内容
            List<string> imagePaths = new List<string>(); // 文件存储路径

            // 将图片写入磁盘
            if(Request.Form.Files.Count > 0)
            {
                foreach(var file in Request.Form.Files)
                {
                    if (file.Length <= 0) continue;
                    var extension = Path.GetExtension(file.FileName);
                    var guid = Guid.NewGuid().ToString();

                    var root = _hostingEnv.WebRootPath;
                    string filename = guid + extension;                           // 新的文件名称<guid>
                    var relativePath = $@"\upload\service_record\{ filename}";
                    var fullPath = $@"{root}{relativePath}";   // 新的存储路径
                    if (!Directory.Exists(Path.GetDirectoryName(fullPath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                    }
                    using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyToAsync(stream);
                    }
                    imagePaths.Add(relativePath);
                }
            }
            // 将服务记录写入数据库
            Models.Customer.CustomerServiceRecord serviceRecord = new Models.Customer.CustomerServiceRecord();
            serviceRecord.CustomerId = customerId;
            serviceRecord.ServiceContent = content;
            if(imagePaths.Count > 0)
            {
                serviceRecord.Images = string.Join(",", imagePaths);
            }
            serviceRecord.AppendUserId = _AppUser.Id;

            string apiUrl = _AppConfig.WebApiHost + "/api/Customer/InsertServiceRecord";
            int identityId = APIInvoker.Post<int>(apiUrl, serviceRecord);
            TempData["result"] = identityId > 0;

            return RedirectToAction("Detail", "Customer", new { id = customerId });
        }


        // id=CustomerServiceRecord表主键
        public IActionResult RecordDetail(int id)
        {
            string apiUrl = _AppConfig.WebApiHost + "/api/Customer/CustomerServiceRecord?id=" + id;
            var serviceRecord = APIInvoker.Get<Models.Customer.CustomerServiceRecord>(apiUrl);

            return View(serviceRecord);
        }


        /// <summary>
        /// ajax:删除一条服务记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public bool DeleteServiceRecord(int id)
        {
            string apiUrl = _AppConfig.WebApiHost + "/api/Customer/DeleteServiceRecord?id=" + id;
            bool res = APIInvoker.Get<bool>(apiUrl);

            return res;
        }
    }
}