using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Orion.CRM.WebApp.Controllers
{
    /// <summary>
    /// ҵ��ͳ�ƿ�����
    /// </summary>
    public class SalesController : BaseController
    {
        public IActionResult Month()
        {
            return View();
        }
    }
}