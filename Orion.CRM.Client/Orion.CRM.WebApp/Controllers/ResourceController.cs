using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orion.CRM.WebApp.App_Data;

namespace Orion.CRM.WebApp.Controllers
{
    /// <summary>
    /// ��Դ���������
    /// </summary>
    public class ResourceController : BaseController
    {    
        // ��Դ�б�
        public IActionResult List()
        {
            AppConfig _c = _AppConfig;
            return View();
        }

        // ������Դ
        public IActionResult Public()
        {
            return View();
        }

        // ��������Դ
        public IActionResult Unvalued()
        {
            return View();
        }

        // ��Դ¼��
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

        // ��Դ��������
        public IActionResult BatchImport()
        {
            return View();
        }

        // ��������
        public IActionResult Assign()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AssignHandler()
        {
            return View();
        }

        // ��Դ��Դͳ��
        public IActionResult SourceStat()
        {
            return View();
        }
    }
}