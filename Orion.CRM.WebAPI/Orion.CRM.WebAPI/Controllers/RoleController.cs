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

        [HttpPost]
        public APIDataResult InsertRolePage([FromBody]Entity.RolePage rolePage)
        {
            try {
                int identityId = service.InsertRolePage(rolePage);
                APIDataResult dataResult = new APIDataResult(200, identityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

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

        public APIDataResult DeleteRoleMenuByRoleId(int roleId)
        {
            try {
                bool res = service.DeleteRoleMenuByRoleId(roleId);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

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

        public APIDataResult DeleteRolePageByRoleId(int roleId)
        {
            try {
                bool res = service.DeleteRolePageByRoleId(roleId);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult DeleteRolePageByPageId(int pageId)
        {
            try {
                bool res = service.DeleteRolePageByPageId(pageId);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

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

        public APIDataResult GetRolePagesByRoleId(int roleId)
        {
            try {
                IEnumerable<Entity.RolePage> rolePages = service.GetRolePagesByRoleId(roleId);
                APIDataResult dataResult = new APIDataResult(200, rolePages);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        public APIDataResult GetRolePagesByPageId(int pageId)
        {
            try {
                IEnumerable<Entity.RolePage> rolePages = service.GetRolePagesByPageId(pageId);
                APIDataResult dataResult = new APIDataResult(200, rolePages);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }


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
    }
}