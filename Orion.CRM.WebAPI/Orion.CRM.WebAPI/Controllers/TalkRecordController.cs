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
    public class TalkRecordController : Controller
    {
        private TalkRecordAppService service = new TalkRecordAppService();

        [HttpPost]
        public APIDataResult InsertTalkRecord([FromBody]Entity.TalkRecord record)
        {
            try {
                int identityId = service.InsertTalkRecord(record);
                APIDataResult dataResult = new APIDataResult(200, identityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult DeleteTalkRecord(int id)
        {
            try {
                bool res = service.DeleteTalkRecord(id);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult GetRecordsByResourceId(int resourceId)
        {
            try {
                IEnumerable<Entity.TalkRecord> sources = service.GetRecordsByResourceId(resourceId);
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