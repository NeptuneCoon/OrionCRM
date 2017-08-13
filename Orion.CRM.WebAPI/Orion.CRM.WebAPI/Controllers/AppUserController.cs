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

        public APIDataResult GetAllUsersByGroupId(int groupId)
        {
            try {
                IEnumerable<Entity.AppUser> users = service.GetAllUsersByGroupId(groupId);
                APIDataResult dataResult = new APIDataResult(200, users);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult GetAllUsersByProjectId(int projectId)
        {
            try {
                IEnumerable<Entity.AppUserComplex> users = service.GetAllUsersByProjectId(projectId);
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

        [HttpGet]
        public APIDataResult UpdatePassword(string userId, string password)
        {
            try {
                bool res = service.UpdatePassword(userId, password);
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

        [HttpPost]
        public APIDataResult InsertUserRole([FromBody]Entity.UserRole userRole)
        {
            try {
                int idnetityId = service.InsertUserRole(userRole);
                APIDataResult dataResult = new APIDataResult(200, idnetityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }


        [HttpPost]
        public APIDataResult UpdateUserRole([FromBody]Entity.UserRole userRole)
        {
            try {
                bool res = service.UpdateUserRole(userRole);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult GetUserProject(int userId)
        {
            try {
                var entity = service.GetUserProject(userId);
                APIDataResult dataResult = new APIDataResult(200, entity);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult InsertUserProject([FromBody]Entity.UserProject userProject)
        {
            try {
                int idnetityId = service.InsertUserProject(userProject);
                APIDataResult dataResult = new APIDataResult(200, idnetityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult UpdateUserProject([FromBody]Entity.UserProject userProject)
        {
            try {
                bool res = service.UpdateUserProject(userProject);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult DeleteUserProject(int userId)
        {
            try {
                int count = service.DeleteUserProject(userId);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult GetUserGroup(int userId)
        {
            try {
                var entity = service.GetUserGroup(userId);
                APIDataResult dataResult = new APIDataResult(200, entity);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult InsertUserGroup([FromBody]Entity.UserGroup userGroup)
        {
            try {
                int idnetityId = service.InsertUserGroup(userGroup);
                APIDataResult dataResult = new APIDataResult(200, idnetityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult UpdateUserGroup([FromBody]Entity.UserGroup userGroup)
        {
            try {
                bool res = service.UpdateUserGroup(userGroup);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult DeleteUserGroup(int userId)
        {
            try {
                int count = service.DeleteUserGroup(userId);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // 删除用户及相关数据
        public APIDataResult DeleteUser(int userId)
        {
            try {
                int count = service.DeleteUser(userId);
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