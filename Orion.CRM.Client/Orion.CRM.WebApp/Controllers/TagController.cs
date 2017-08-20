using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orion.CRM.WebTools;

namespace Orion.CRM.WebApp.Controllers
{
    public class TagController : BaseController
    {
        public IActionResult List()
        {
            string url = _AppConfig.WebApiHost + "api/Tag/GetTagsByUserId?userId=" + _AppUser.Id;
            List<Models.Tag.Tag> list = APIInvoker.Get<List<Models.Tag.Tag>>(url);

            return View(list);
        }

        [HttpPost]
        public int Insert(Models.Tag.Tag tag)
        {
            if (tag != null) {
                tag.TagName = tag.TagName.Trim();
                tag.UserId = _AppUser.Id;
                tag.CreateTime = DateTime.Now;
                tag.UpdateTime = DateTime.Now;

                string url = _AppConfig.WebApiHost + "api/Tag/InsertTag";
                int identityId = APIInvoker.Post<int>(url, tag);
                return identityId;
            }

            return 0;
        }

        [HttpPost]
        public bool Update(Models.Tag.Tag tag)
        {
            if (tag != null && tag.Id > 0) {
                tag.TagName = tag.TagName.Trim();
                tag.UpdateTime = DateTime.Now;

                string url = _AppConfig.WebApiHost + "api/Tag/UpdateTag";
                bool result = APIInvoker.Post<bool>(url, tag);
                return result;
            }
            return false;
        }

        [HttpGet]
        public bool Delete(int tagId)
        {
            string url = _AppConfig.WebApiHost + "api/Tag/DeleteTag?id=" + tagId;
            bool result = APIInvoker.Get<bool>(url);
            return result;
        }

        // Ajax重新加载页面
        [HttpGet]
        public List<Models.Tag.Tag> ReloadList()
        {
            string url = _AppConfig.WebApiHost + "api/Tag/GetTagsByUserId?userId=" + _AppUser.Id;
            List<Models.Tag.Tag> list = APIInvoker.Get<List<Models.Tag.Tag>>(url);

            return list;
        }
    }
}