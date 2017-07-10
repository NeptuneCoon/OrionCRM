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
    public class ResourceController : Controller
    {
        private ResourceAppService service = new ResourceAppService();

        [HttpPost]
        public APIDataResult InsertResource([FromBody]Entity.Resource resource)
        {
            try {
                int identityId = service.InsertResource(resource);
                APIDataResult dataResult = new APIDataResult(200, identityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult UpdateReource([FromBody]Entity.Resource resource)
        {
            try {
                bool res = service.UpdateResource(resource);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }


        // 软删除一条资源
        public APIDataResult DeleteResource(int id)
        {
            try {
                bool res = service.DeleteResource(id);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // 恢复一条资源
        public APIDataResult RestoreResource(int id)
        {
            try {
                bool res = service.RestoreResource(id);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult InsertResourceOrganization([FromBody]Entity.ResourceOrganization resourceOrg)
        {
            try {
                int identityId = service.InsertResourceOrganization(resourceOrg);
                APIDataResult dataResult = new APIDataResult(200, identityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult InsertResourceProject([FromBody]Entity.ResourceProject resourceProject)
        {
            try {
                int identityId = service.InsertResourceProject(resourceProject);
                APIDataResult dataResult = new APIDataResult(200, identityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        /// <summary>
        /// 根据筛选条件查询资源
        /// </summary>
        /// <param name="param">筛选条件对象</param>
        /// <returns></returns>
        [HttpPost]
        public APIDataResult GetResourcesByCondition([FromBody]Entity.ResourceSearchParams param)
        {
            try {
                IEnumerable<Entity.Resource> resources = service.GetResourcesByCondition(param);
                APIDataResult dataResult = new APIDataResult(200, resources);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        /// <summary>
        /// 根据筛选条件获取符合条件的资源个数
        /// </summary>
        /// <param name="param">筛选条件对象</param>
        /// <returns></returns>
        [HttpPost]
        public APIDataResult GetResourceCountByCondition([FromBody]Entity.ResourceSearchParams param)
        {
            try {
                int count = service.GetResourceCountByCondition(param);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // 根据Id获取资源
        public APIDataResult GetResourceById(int id)
        {
            try {
                Entity.Resource resource = service.GetResourceById(id);
                APIDataResult dataResult = new APIDataResult(200, resource);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // 判断资源是否存在
        public APIDataResult IsResourceExist(int orgId, string mobile, string tel, string qq, string wechat)
        {
            try {
                bool result = service.IsResourceExist(orgId, mobile, tel, qq, wechat);
                APIDataResult dataResult = new APIDataResult(200, result);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // 设置资源状态
        public APIDataResult SetResourceStatus(int resourceId, int status)
        {
            try {
                bool result = service.SetResourceStatus(resourceId, status);
                APIDataResult dataResult = new APIDataResult(200, result);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // 添加资源和业务组之间的关系
        [HttpPost]
        public APIDataResult InsertResourceGroup([FromBody]Entity.ResourceGroup resourceGroup)
        {
            try {
                int identityId = service.InsertResourceGroup(resourceGroup);
                APIDataResult dataResult = new APIDataResult(200, identityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // 修改资源和业务组之间的关系
        [HttpPost]
        public APIDataResult UpdateResourceGroup([FromBody]Entity.ResourceGroup resourceGroup)
        {
            try {
                bool res = service.UpdateResourceGroup(resourceGroup);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // 获取资源和业务组之间的关系
        public APIDataResult GetResourceGroup(int resourceId)
        {
            try {
                Entity.ResourceGroup resourceGroup = service.GetResourceGroup(resourceId);
                APIDataResult dataResult = new APIDataResult(200, resourceGroup);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // 添加资源和用户之间的关系
        [HttpPost]
        public APIDataResult InsertResourceUser([FromBody]Entity.ResourceUser resourceUser)
        {
            try {
                int identityId = service.InsertResourceUser(resourceUser);
                APIDataResult dataResult = new APIDataResult(200, identityId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // 修改资源和用户之间的关系
        [HttpPost]
        public APIDataResult UpdateResourceUser([FromBody]Entity.ResourceUser resourceUser)
        {
            try {
                bool res = service.UpdateResourceUser(resourceUser);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // 获取资源和用户之间的关系
        public APIDataResult GetResourceUser(int resourceId)
        {
            try {
                Entity.ResourceUser resourceUser = service.GetResourceUser(resourceId);
                APIDataResult dataResult = new APIDataResult(200, resourceUser);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // 获取未分配至业务组的资源
        public APIDataResult GetGroupUnAssignedResources(int projectId)
        {
            try {
                IEnumerable<Entity.UnassignedResource> resourceIds = service.GetGroupUnAssignedResources(projectId);
                APIDataResult dataResult = new APIDataResult(200, resourceIds);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // 获取未分配至业务组的资源个数
        public APIDataResult GetGroupUnAssignedResourceCount(int projectId)
        {
            try {
                int count = service.GetGroupUnAssignedResourceCount(projectId);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // 获取未分配至业务员的资源个数
        public APIDataResult GetUserUnAssignedResourceCount(int orgId)
        {
            try {
                int count = service.GetUserUnAssignedResourceCount(orgId);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // 获取资源来源情况
        public APIDataResult GetResourceSourceStat(int orgId)
        {
            try {
                IEnumerable<Entity.ResourceSource> query = service.GetResourceSourceStat(orgId);
                APIDataResult dataResult = new APIDataResult(200, query);
                return dataResult;
            }
            catch(Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }
    }
}