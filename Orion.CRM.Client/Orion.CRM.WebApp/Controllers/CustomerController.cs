using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Orion.CRM.WebApp.Controllers
{
    /// <summary>
    /// �ͻ�������
    /// </summary>
    public class CustomerController : BaseController
    {
        public IActionResult Search()
        {
            return View();
        }
    }
}