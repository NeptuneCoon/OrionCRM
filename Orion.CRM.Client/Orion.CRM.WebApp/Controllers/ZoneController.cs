using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orion.CRM.WebApp.App_Data;
using Orion.CRM.WebTools;

namespace Orion.CRM.WebApp.Controllers
{
    public class ZoneController : BaseController
    {
        public IActionResult Index()
        {
            ViewBag.Projects = AppDTO.GetProjectsFromDb(_AppUser.OrgId);

            return View();
        }


        /// <summary>
        /// ajax:根据省/直辖市获取客户
        /// </summary>
        /// <param name="agentZone1"></param>
        /// <returns></returns>
        [HttpGet]
        public List<Models.Customer.CustomerModel> GetCustomersByZone(string pid,string bid, string z1)
        {
            if (string.IsNullOrEmpty(pid) || string.IsNullOrEmpty(pid)) return null;
            string apiUrl = _AppConfig.WebApiHost + $"/api/Customer/GetCustomersByZone?pid={pid}&bid={bid}&z1={z1}";
            var customers = APIInvoker.Get<List<Models.Customer.CustomerModel>>(apiUrl);
            return customers;
        }
    }
}