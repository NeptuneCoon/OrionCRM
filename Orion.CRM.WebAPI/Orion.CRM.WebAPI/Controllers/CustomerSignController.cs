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
    public class CustomerSignController : Controller
    {
        private CustomerSignAppService service = new CustomerSignAppService();

        [HttpPost]
        public APIDataResult InsertSign([FromBody]Entity.CustomerSign sign)
        {
            try {
                int identityId = service.InsertSign(sign);
                APIDataResult dataResult = new APIDataResult(200, identityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult DeleteSign(int resourceId)
        {
            try {
                int count = service.DeleteSign(resourceId);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult GetSignByResourceId(int resourceId)
        {
            try {
                var entity = service.GetSignByResourceId(resourceId);
                APIDataResult dataResult = new APIDataResult(200, entity);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }
    }
}