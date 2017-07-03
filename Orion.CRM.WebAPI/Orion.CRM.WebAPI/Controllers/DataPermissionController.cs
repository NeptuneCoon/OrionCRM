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
    public class DataPermissionController : Controller
    {
        DataPermissionAppService service = new DataPermissionAppService();

        public APIDataResult GetDataPermissionCategories()
        {
            try {
                var permissionCategories = service.GetDataPermissionCategories();
                APIDataResult dataResult = new APIDataResult(200, permissionCategories);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult GetRoleDataPermissions(int roleId)
        {
            try {
                var rolePermissions = service.GetRoleDataPermissions(roleId);
                APIDataResult dataResult = new APIDataResult(200, rolePermissions);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult DeleteRoleDataPermissions(int roleId)
        {
            try {
                int count = service.DeleteRoleDataPermissions(roleId);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        //[HttpPost]
        //public APIDataResult InsertRoleDataPermission([FromBody]Entity.RoleDataPermission rolePermission)
        //{
        //    try {
        //        int identityId = service.InsertRoleDataPermission(rolePermission);
        //        APIDataResult dataResult = new APIDataResult(200, identityId);
        //        return dataResult;
        //    }
        //    catch (Exception ex) {
        //        APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
        //        return dataResult;
        //    }
        //}

        [HttpPost]
        public APIDataResult RoleDataPermissionBatchInsert([FromBody]IEnumerable<Entity.RoleDataPermission> rolePermissions)
        {
            try {
                bool result = service.RoleDataPermissionBatchInsert(rolePermissions);
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