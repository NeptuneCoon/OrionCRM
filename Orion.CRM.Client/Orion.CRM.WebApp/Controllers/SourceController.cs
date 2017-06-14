using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orion.CRM.WebTools;

namespace Orion.CRM.WebApp.Controllers
{
    /// <summary>
    /// 资源来源控制器
    /// </summary>
    public class SourceController : BaseController
    {
        public IActionResult List()
        {
            string url = _AppConfig.WebAPIHost + "api/ResourceSource/GetSourcesByOrgId?OrgId=" + _AppUser.OrgId;
            List<Models.Source.ResourceSource> list = APIInvoker.Get<List<Models.Source.ResourceSource>>(url);

            ViewBag.OperateResult = Request.Query["operateResult"].ToString();

            return View(list);
        }

        [HttpPost]
        public int Insert([FromBody]Models.Source.ResourceSource source)
        {
            if (source != null) {
                source.CreateTime = DateTime.Now;
                source.UpdateTime = DateTime.Now;
                source.OrgId = _AppUser.OrgId;

                string url = _AppConfig.WebAPIHost + "api/ResourceSource/InsertSource";
                int identityId = APIInvoker.Post<int>(url, source);
                return identityId;
            }

            return 0;
        }

        [HttpPost]
        public bool Update([FromBody]Models.Source.ResourceSource source)
        {
            if (source != null && source.Id > 0) {
                source.UpdateTime = DateTime.Now;
                source.OrgId = _AppUser.OrgId;

                string url = _AppConfig.WebAPIHost + "api/ResourceSource/UpdateSource";
                bool result = APIInvoker.Post<bool>(url, source);
                return result;
            }
            return false; 
        }

        public bool Delete(int id)
        {
            if (id > 0) { 
                string url = _AppConfig.WebAPIHost + "api/ResourceSource/DeleteSource?id=" + id;
                bool result = APIInvoker.Get<bool>(url);
                return result;
            }
            return false;
        }
    }
}