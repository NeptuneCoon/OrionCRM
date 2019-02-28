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
        public List<Models.Customer.CustomerModel> GetCustomersByZone1(string agentZone1)
        {
            if (string.IsNullOrEmpty(agentZone1)) return null;
            string apiUrl = _AppConfig.WebApiHost + "/api/Customer/GetCustomersByZone1?agentZone1=" + agentZone1;
            var customers = APIInvoker.Get<List<Models.Customer.CustomerModel>>(apiUrl);
            return customers;
        }
    }
}