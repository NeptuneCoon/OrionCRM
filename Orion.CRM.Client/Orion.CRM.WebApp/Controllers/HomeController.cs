using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orion.CRM.WebApp.Models;

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

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
