using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Orion.CRM.WebApp.Controllers
{
    public class GroupController : BaseController
    {
        public IActionResult List()
        {
            return View();
        }
    }
}