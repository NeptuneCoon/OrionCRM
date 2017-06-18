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
    public class AppUserController : Controller
    {
        private AppUserAppService service = new AppUserAppService();

        public APIDataResult GetUsers(int pageIndex, int pageSize)
        {
            try {
                IEnumerable<Entity.AppUser> users = service.GetUsers(pageIndex, pageSize);
                APIDataResult dataResult = new APIDataResult(200, users);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult GetUsersByOrgId(int pageIndex, int pageSize, int orgId)
        {
            try {
                IEnumerable<Entity.AppUser> users = service.GetUsersByOrgId(pageIndex, pageSize, orgId);
                APIDataResult dataResult = new APIDataResult(200, users);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult GetUserById(int id)
        {
            try {
                Entity.AppUser user = service.GetUserById(id);
                APIDataResult dataResult = new APIDataResult(200, user);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult GetUserByUserName(string userName)
        {
            try {
                Entity.AppUser user = service.GetUserByUserName(userName);
                APIDataResult dataResult = new APIDataResult(200, user);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult InsertUser([FromBody]Entity.AppUser user)
        {
            try {
                int identityId = service.InsertUser(user);
                APIDataResult dataResult = new APIDataResult(200, identityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult UpdateUser([FromBody]Entity.AppUser user)
        {
            try {
                bool res = service.UpdateUser(user);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult GetUserCount()
        {
            try {
                int count = service.GetUserCount();
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult GetUserCountByOrgId(int orgId)
        {
            try {
                int count = service.GetUserCountByOrgId(orgId);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }
    }
}