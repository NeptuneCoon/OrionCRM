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
    public class OrganizationController : Controller
    {
        private OrganizationAppService service = new OrganizationAppService();

        //[HttpGet("{id}")]
        public APIDataResult GetOrganizationById(int id)
        {
            try {
                Entity.Organization org = service.GetOrganizationById(id);
                APIDataResult dataResult = new APIDataResult(200, org);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult GetAllOrganizations()
        {
            try {
                IEnumerable<Entity.Organization> orgs = service.GetAllOrganizations();
                APIDataResult dataResult = new APIDataResult(200, orgs);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult CreateOrganization([FromBody]Entity.Organization org)
        {
            try {
                int identityId = service.InsertOrganization(org);
                APIDataResult dataResult = new APIDataResult(200, identityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult UpdateOrganization([FromBody]Entity.Organization org)
        {
            try {
                bool res = service.UpdateOrganization(org);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }
    }
}