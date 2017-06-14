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
    public class ResourceSourceController : Controller
    {
        private ResourceSourceAppService service = new ResourceSourceAppService();

        [HttpPost]
        public APIDataResult InsertSource([FromBody]Entity.ResourceSource source)
        {
            try {
                int identityId = service.InsertSource(source);
                APIDataResult dataResult = new APIDataResult(200, identityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult UpdateSource([FromBody]Entity.ResourceSource source)
        {
            try {
                bool res = service.UpdateSource(source);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult DeleteSource(int id)
        {
            try {
                bool res = service.DeleteSource(id);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult GetSourcesByOrgId(int orgId)
        {
            try {
                IEnumerable<Entity.ResourceSource> sources = service.GetSourcesByOrgId(orgId);
                APIDataResult dataResult = new APIDataResult(200, sources);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }
    }
}