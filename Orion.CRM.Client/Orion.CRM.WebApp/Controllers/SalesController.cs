using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Orion.CRM.WebApp.Controllers
{
    /// <summary>
    /// 业绩统计控制器
    /// </summary>
    public class SalesController : BaseController
    {
        public IActionResult Month()
        {
            return View();
        }

        public IActionResult Quarter()
        {
            return View();
        }

        public IActionResult Year()
        {
            return View();
        }

        // 业绩概览
        public IActionResult Overview()
        {
            return View();
        }

        // 组业绩分析
        public IActionResult Group()
        {
            return View();
        }


    }
}