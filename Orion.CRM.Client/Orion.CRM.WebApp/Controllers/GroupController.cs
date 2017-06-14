using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Orion.CRM.WebApp.Controllers
{
    /// <summary>
    /// 业务组控制器
    /// </summary>
    public class GroupController : BaseController
    {
        public IActionResult List()
        {
            return View();
        }
    }
}