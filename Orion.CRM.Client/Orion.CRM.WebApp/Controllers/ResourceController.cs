using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orion.CRM.WebApp.App_Data;

namespace Orion.CRM.WebApp.Controllers
{
    /// <summary>
    /// 资源管理控制器
    /// </summary>
    public class ResourceController : BaseController
    {    
        // 资源列表
        public IActionResult List()
        {
            AppConfig _c = _AppConfig;
            return View();
        }

        // 公共资源
        public IActionResult Public()
        {
            return View();
        }

        // 无意向资源
        public IActionResult Unvalued()
        {
            return View();
        }

        // 资源录入
        public IActionResult Add()
        {
            Models.Resource.ResourceViewModel viewModel = new Models.Resource.ResourceViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddHandler()
        {
            return View();
        }

        // 资源批量导入
        public IActionResult BatchImport()
        {
            return View();
        }

        // 批量分配
        public IActionResult Assign()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AssignHandler()
        {
            return View();
        }

        // 资源来源统计
        public IActionResult SourceStat()
        {
            return View();
        }
    }
}