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
    public class CRMLogController : Controller
    {
        private CRMLogAppService service = new CRMLogAppService();

        [HttpPost]
        public APIDataResult InsertActionLog([FromBody]Entity.CRMLog.ActionLog log)
        {
            try {
                long identityId = service.InsertActionLog(log);
                APIDataResult dataResult = new APIDataResult(200, identityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult InsertErrorLog([FromBody]Entity.CRMLog.ErrorLog log)
        {
            try {
                long identityId = service.InsertErrorLog(log);
                APIDataResult dataResult = new APIDataResult(200, identityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult InsertLoginLog([FromBody]Entity.CRMLog.LoginLog log)
        {
            try {
                long identityId = service.InsertLoginLog(log);
                APIDataResult dataResult = new APIDataResult(200, identityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }
    }
}