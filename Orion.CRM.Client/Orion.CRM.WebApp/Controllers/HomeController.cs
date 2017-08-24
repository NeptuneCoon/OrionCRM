using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Orion.CRM.WebApp.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Version()
        {
            return View();
        }

        public IActionResult Help()
        {
            return View();
        }
    }
}
