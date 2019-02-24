using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orion.CRM.WebApp.App_Data;

namespace Orion.CRM.WebApp.Controllers
{
    public class CustomerController : BaseController
    {
        public IActionResult All()
        {
            return View();
        }

        public IActionResult My()
        {
            return View();
        }

        public IActionResult Add()
        {
            ViewBag.Projects = AppDTO.GetProjectsFromDb(_AppUser.OrgId);
            
            return View();
        }

        public IActionResult Detail()
        {
            return View();
        }

    }
}