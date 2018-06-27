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
    public class ResourceNoteController : Controller
    {
        private ResourceNoteAppService service = new ResourceNoteAppService();

        [HttpPost]
        public APIDataResult InsertResourceNote([FromBody]Entity.ResourceNote note)
        {
            try {
                int identityId = service.InsertResourceNote(note);
                APIDataResult dataResult = new APIDataResult(200, identityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult DeleteResourceNote(int id)
        {
            try {
                bool res = service.DeleteResourceNote(id);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult GetNotesByResourceId(int resourceId)
        {
            try {
                IEnumerable<Entity.ResourceNote> sources = service.GetNotesByResourceId(resourceId);
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