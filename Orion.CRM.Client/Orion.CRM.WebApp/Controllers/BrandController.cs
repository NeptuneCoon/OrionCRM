using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orion.CRM.WebApp.App_Data;
using Orion.CRM.WebTools;

namespace Orion.CRM.WebApp.Controllers
{
    public class BrandController : BaseController
    {
        public IActionResult Index()
        {
            string url = _AppConfig.WebApiHost + "/api/Brand/GetAllBrands";
            List<Models.Brand.Brand> brands = APIInvoker.Get<List<Models.Brand.Brand>>(url);
            ViewBag.Projects = AppDTO.GetProjectsFromDb(_AppUser.OrgId);

            return View(brands);
        }

        [HttpPost]
        public int Insert(Models.Brand.Brand brand)
        {
            if (brand != null) {
                brand.BrandName = brand.BrandName.Trim();
                string url = _AppConfig.WebApiHost + "/api/Brand/InsertBrand";
                int identityId = APIInvoker.Post<int>(url, brand);
                return identityId;
            }

            return 0;
        }

        [HttpPost]
        public bool Update(Models.Brand.Brand brand)
        {
            if (brand != null && brand.Id > 0) {
                brand.BrandName = brand.BrandName.Trim();

                string url = _AppConfig.WebApiHost + "/api/Brand/UpdateBrand";
                bool result = APIInvoker.Post<bool>(url, brand);
                return result;
            }
            return false;
        }


        // Ajax重新加载页面
        [HttpGet]
        public List<Models.Brand.Brand> ReloadList()
        {
            string url = _AppConfig.WebApiHost + "/api/Brand/GetAllBrands";
            List<Models.Brand.Brand> list = APIInvoker.Get<List<Models.Brand.Brand>>(url);
            return list;
        }

        // Ajax获取品牌
        [HttpGet]
        public List<Models.Brand.Brand> GetBrandsByProjectId(int projectId)
        {
            string url = _AppConfig.WebApiHost + "/api/Brand/GetBrandsByProjectId?projectId=" + projectId;
            List<Models.Brand.Brand> list = APIInvoker.Get<List<Models.Brand.Brand>>(url);
            return list;
        }
    }
}