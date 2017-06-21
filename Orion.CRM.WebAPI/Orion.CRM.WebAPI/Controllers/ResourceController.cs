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
    }
}