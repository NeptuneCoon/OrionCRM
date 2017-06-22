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
            string url = _AppConfig.WebApiHost + "api/ResourceSource/GetSourcesByOrgId?OrgId=" + _AppUser.OrgId;
            List<Models.Source.ResourceSource> list = APIInvoker.Get<List<Models.Source.ResourceSource>>(url);

            return View(list);
        }

        [HttpPost]
        public int Insert(Models.Source.ResourceSource source)
        {
            if (source != null) {
                source.CreateTime = DateTime.Now;
                source.UpdateTime = DateTime.Now;
                source.OrgId = _AppUser.OrgId;

                string url = _AppConfig.WebApiHost + "api/ResourceSource/InsertSource";
                int identityId = APIInvoker.Post<int>(url, source);
                return identityId;
            }

            return 0;
        }

        [HttpPost]
        public bool Update(Models.Source.ResourceSource source)
        {
            if (source != null && source.Id > 0) {
                source.UpdateTime = DateTime.Now;
                source.OrgId = _AppUser.OrgId;

                string url = _AppConfig.WebApiHost + "api/ResourceSource/UpdateSource";
                bool result = APIInvoker.Post<bool>(url, source);
                return result;
            }
            return false; 
        }

        public bool Delete(int id)
        {
            if (id > 0) { 
                string url = _AppConfig.WebApiHost + "api/ResourceSource/DeleteSource?id=" + id;
                bool result = APIInvoker.Get<bool>(url);
                return result;
            }
            return false;
        }

        // Ajax重新加载页面
        [HttpGet]
        public List<Models.Source.ResourceSource> ReloadList()
        {
            string url = _AppConfig.WebApiHost + "api/ResourceSource/GetSourcesByOrgId?OrgId=" + _AppUser.OrgId;
            List<Models.Source.ResourceSource> list = APIInvoker.Get<List<Models.Source.ResourceSource>>(url);
            return list;
        }
    }
}