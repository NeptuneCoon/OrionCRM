using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Orion.CRM.WebApp.Controllers
{
    public class CustomerController : Controller
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
            return View();
        }

        public IActionResult Detail()
        {
            return View();
        }
    }
}