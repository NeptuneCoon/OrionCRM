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
    public class RoleController : Controller
    {
        private RoleAppService service = new RoleAppService();

        [HttpGet]
        public APIDataResult GetRoleById(int id)
        {
            try {
                Entity.Role role = service.GetRoleById(id);
                APIDataResult dataResult = new APIDataResult(200, role);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult InsertRole([FromBody]Entity.Role role)
        {
            try {
                int identityId = service.InsertRole(role);
                APIDataResult dataResult = new APIDataResult(200, identityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult UpdateRole([FromBody]Entity.Role role)
        {
            try {
                bool res = service.UpdateRole(role);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult DeleteRole(int id)
        {
            try {
                bool res = service.DeleteRole(id);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult InsertRoleMenu([FromBody]Entity.RoleMenu roleMenu)
        {
            try {
                int identityId = service.InsertRoleMenu(roleMenu);
                APIDataResult dataResult = new APIDataResult(200, identityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult DeleteRoleMenuById(int id)
        {
            try {
                bool res = service.DeleteRoleMenuById(id);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult DeleteRoleMenuByRoleId(int roleId)
        {
            try {
                int count = service.DeleteRoleMenuByRoleId(roleId);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult DeleteRoleMenuByMenuId(int menuId)
        {
            try {
                bool res = service.DeleteRoleMenuByMenuId(menuId);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult GetRoles(int pageIndex, int pageSize)
        {
            try {
                IEnumerable<Entity.Role> roles = service.GetRoles(pageIndex, pageSize);
                APIDataResult dataResult = new APIDataResult(200, roles);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult GetRoleCount()
        {
            try {
                int count = service.GetRoleCount();
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult GetRolesByOrgId(int pageIndex, int pageSize, int orgId)
        {
            try {
                IEnumerable<Entity.Role> roles = service.GetRolesByOrgId(pageIndex, pageSize, orgId);
                APIDataResult dataResult = new APIDataResult(200, roles);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult GetRoleCountByOrgId(int orgId)
        {
            try {
                int count = service.GetRoleCountByOrgId(orgId);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult GetRoleMenusByRoleId(int roleId)
        {
            try {
                IEnumerable<Entity.RoleMenu> roleMenus = service.GetRoleMenusByRoleId(roleId);
                APIDataResult dataResult = new APIDataResult(200, roleMenus);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult GetRoleMenusByOrgId(int orgId)
        {
            try {
                IEnumerable<Entity.RoleMenu> roleMenus = service.GetRoleMenusByOrgId(orgId);
                APIDataResult dataResult = new APIDataResult(200, roleMenus);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult GetRoleMenusByMenuId(int menuId)
        {
            try {
                IEnumerable<Entity.RoleMenu> roleMenus = service.GetRoleMenusByMenuId(menuId);
                APIDataResult dataResult = new APIDataResult(200, roleMenus);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult GetAllRoleMenus()
        {
            try {
                IEnumerable<Entity.RoleMenuComplex> roleMenuComplexs = service.GetAllRoleMenus();
                APIDataResult dataResult = new APIDataResult(200, roleMenuComplexs);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult GetComplexRoleMenusByRoleId(int roleId)
        {
            try {
                IEnumerable<Entity.RoleMenuComplex> roleMenuComplexs = service.GetComplexRoleMenusByRoleId(roleId);
                APIDataResult dataResult = new APIDataResult(200, roleMenuComplexs);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult GetAllComplexRoleMenus()
        {
            try {
                IEnumerable<Entity.RoleMenuComplex> roleMenuComplexs = service.GetAllComplexRoleMenus();
                APIDataResult dataResult = new APIDataResult(200, roleMenuComplexs);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult RoleMenuBatchInsert([FromBody]IEnumerable<Entity.RoleMenu> roleMenus)
        {
            try {
                bool res = service.RoleMenuBatchInsert(roleMenus);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult GetUserCountByRoleId(int roleId)
        {
            try {
                int count = service.GetUserCountByRoleId(roleId);
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