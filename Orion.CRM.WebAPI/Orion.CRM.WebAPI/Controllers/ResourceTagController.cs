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
    public class ResourceTagController : Controller
    {
        private ResourceTagAppService service = new ResourceTagAppService();
        [HttpPost]
        public APIDataResult InsertResourceTag([FromBody]Entity.ResourceTag resourceTag)
        {
            try {
                int identityId = service.InsertResourceTag(resourceTag);
                APIDataResult dataResult = new APIDataResult(200, identityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult DeleteResourceTag(int resourceId)
        {
            try {
                bool result = service.DeleteResourceTag(resourceId);
                APIDataResult dataResult = new APIDataResult(200, result);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // 批量删除资源和标签的关系
        public APIDataResult BatchDeleteResourceTag(string resourceIds)
        {
            try {
                int count = service.BatchDeleteResourceTag(resourceIds);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult ResourceTagBatchInsert([FromBody]IEnumerable<Entity.ResourceTagBatchInsert> resourceTags)
        {
            try {
                bool result = service.ResourceTagBatchInsert(resourceTags);
                APIDataResult dataResult = new APIDataResult(200, result);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }
    }
}