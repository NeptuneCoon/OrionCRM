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
            string url = _AppConfig.WebApiHost + "/api/Source/GetSourcesByOrgId?OrgId=" + _AppUser.OrgId;
            List<Models.Source.Source> list = APIInvoker.Get<List<Models.Source.Source>>(url);

            return View(list);
        }

        [HttpPost]
        public int Insert(Models.Source.Source source)
        {
            if (source != null) {
                source.SourceName = source.SourceName.Trim();
                source.CreateTime = DateTime.Now;
                source.UpdateTime = DateTime.Now;
                source.OrgId = _AppUser.OrgId;

                string url = _AppConfig.WebApiHost + "/api/Source/InsertSource";
                int identityId = APIInvoker.Post<int>(url, source);
                return identityId;
            }

            return 0;
        }

        [HttpPost]
        public bool Update(Models.Source.Source source)
        {
            if (source != null && source.Id > 0) {
                source.SourceName = source.SourceName.Trim();
                source.UpdateTime = DateTime.Now;
                source.OrgId = _AppUser.OrgId;

                string url = _AppConfig.WebApiHost + "/api/Source/UpdateSource";
                bool result = APIInvoker.Post<bool>(url, source);
                return result;
            }
            return false; 
        }

        public bool Delete(int id)
        {
            if (id > 0) {
                // 先设置此来源下所有资源的SourceFrom为null
                string clearApiUrl = _AppConfig.WebApiHost + "/api/Resource/ClearSourceFrom?sourceId=" + id;
                APIInvoker.Get<int>(clearApiUrl);

                // 再执行删除操作
                string deleteApiUrl = _AppConfig.WebApiHost + "/api/Source/DeleteSource?id=" + id;
                bool result = APIInvoker.Get<bool>(deleteApiUrl);
                return result;
            }
            return false;
        }

        public int GetResourceCount(int sourceId)
        {
            string url = _AppConfig.WebApiHost + "/api/Resource/GetResourceCountBySourceFrom?orgId=" + _AppUser.OrgId + "&sourceId=" + sourceId;
            int count = APIInvoker.Get<int>(url);
            return count;
        }

        // Ajax重新加载页面
        [HttpGet]
        public List<Models.Source.Source> ReloadList()
        {
            string url = _AppConfig.WebApiHost + "/api/Source/GetSourcesByOrgId?OrgId=" + _AppUser.OrgId;
            List<Models.Source.Source> list = APIInvoker.Get<List<Models.Source.Source>>(url);
            return list;
        }
    }
}