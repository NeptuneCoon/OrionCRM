using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
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

        [HttpPost]
        public async Task<IActionResult> OnPostUpload([FromServices]IHostingEnvironment environment)
        {
            List<string> list = new List<string>();
            var files = Request.Form.Files;
            string webRootPath = environment.WebRootPath;
            string contentRootPath = environment.ContentRootPath;
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + ".jpg";
                    var path = Path.Combine(environment.WebRootPath + @"\upload", fileName);
                    using (var stream = new FileStream(path, FileMode.CreateNew))
                    {
                        await formFile.CopyToAsync(stream);
                        var url = @"/images/upload/" + fileName;
                        list.Add(url);
                    }
                }
            }

            return new JsonResult(list);
        }

        public IActionResult About()
        {
            string name = HttpContext.Session.GetString("name");
            return View();
        }
    }
}