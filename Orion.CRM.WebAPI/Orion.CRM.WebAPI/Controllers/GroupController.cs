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
    public class GroupController : Controller
    {
        private GroupAppService service = new GroupAppService();

        [HttpPost]
        public APIDataResult InsertGroup([FromBody]Entity.Group group)
        {
            try {
                int identityId = service.InsertGroup(group);
                APIDataResult dataResult = new APIDataResult(200, identityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult UpdateGroup([FromBody]Entity.Group group)
        {
            try {
                bool res = service.UpdateGroup(group);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult DeleteGroup(int id)
        {
            try {
                bool res = service.DeleteGroup(id);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult GetGroupById(int id)
        {
            try {
                Entity.Group group = service.GetGroupById(id);
                APIDataResult dataResult = new APIDataResult(200, group);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult GetGroupsByProjectId(int projectId)
        {
            try {
                IEnumerable<Entity.Group> groups = service.GetGroupsByProjectId(projectId);
                APIDataResult dataResult = new APIDataResult(200, groups);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult GetGroupsByOrgId(int orgId)
        {
            try {
                IEnumerable<Entity.Group> groups = service.GetGroupsByOrgId(orgId);
                APIDataResult dataResult = new APIDataResult(200, groups);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }
    }
}