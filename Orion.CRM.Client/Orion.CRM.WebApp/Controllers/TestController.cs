using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Orion.CRM.WebApp.Controllers
{
    public class TestController : Controller
    {
        private readonly IDistributedCache _distributedCache;
        public TestController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public IActionResult Index()
        {
            _distributedCache.SetString("movie", "星际穿越");

            HttpContext.Session.SetString("name", "gaochong");
            string name = HttpContext.Session.GetString("name");
            return View();
        }

        public IActionResult About()
        {
            string name = HttpContext.Session.GetString("name");
            return View();
        }
    }
}