using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orion.CRM.Application;

namespace Orion.CRM.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class BrandController : Controller
    {
        private BrandAppService service = new BrandAppService();

        [HttpPost]
        public APIDataResult InsertBrand([FromBody]Entity.Brand brand)
        {
            try {
                int identityId = service.InsertBrand(brand);
                APIDataResult dataResult = new APIDataResult(200, identityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult UpdateBrand([FromBody]Entity.Brand brand)
        {
            try {
                bool res = service.UpdateBrand(brand);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult DeleteBrand(int id)
        {
            try {
                bool res = service.DeleteBrand(id);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult GetAllBrands()
        {
            try {
                var datas = service.GetAllBrands();
                APIDataResult dataResult = new APIDataResult(200, datas);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult GetBrandsByProjectId(int projectId)
        {
            try {
                var datas = service.GetBrandsByProjectId(projectId);
                APIDataResult dataResult = new APIDataResult(200, datas);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }
    }
}