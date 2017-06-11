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
    public class ProjectController : Controller
    {
        private ProjectAppService service = new ProjectAppService();

        public APIDataResult GetProjectById(int id)
        {
            try {
                Entity.Project project = service.GetProjectById(id);
                APIDataResult dataResult = new APIDataResult(200, project);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult GetProjectsByOrgId(int orgId)
        {
            try {
                IEnumerable<Entity.Project> projects = service.GetProjectsByOrgId(orgId);
                APIDataResult dataResult = new APIDataResult(200, projects);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult InsertProject([FromBody]Entity.Project project)
        {
            try {
                int identityId = service.InsertProject(project);
                APIDataResult dataResult = new APIDataResult(200, identityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult UpdateProject([FromBody]Entity.Project project)
        {
            try {
                bool res = service.UpdateProject(project);
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