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


        // 删除一条资源
        [HttpGet]
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

        // 删除ResourceUser记录
        [HttpGet]
        public APIDataResult DeleteResourceUserByUserId(int userId)
        {
            try {
                int count = service.DeleteResourceUserByUserId(userId);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // 恢复一条资源
        [HttpGet]
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
        [HttpGet]
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

        /// <summary>
        /// 根据姓名/电话(Mobile/Tel)/微信/QQ查询一条资源
        /// </summary>
        /// <param name="key">查询关键词</param>
        /// <param name="orgId">组织机构Id</param>
        /// <returns></returns>
        [HttpGet]
        public APIDataResult GetResourceByNameMobileWechatQQ(string key, int orgId)
        {
            try {
                Entity.Resource resource = service.GetResourceByNameMobileWechatQQ(key, orgId);
                APIDataResult dataResult = new APIDataResult(200, resource);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // 判断资源是否存在
        [HttpGet]
        public APIDataResult IsResourceExist(int orgId, int projectId, string mobile, string tel, string qq, string wechat)
        {
            try {
                bool result = service.IsResourceExist(orgId, projectId, mobile, tel, qq, wechat);
                APIDataResult dataResult = new APIDataResult(200, result);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // 设置资源状态
        [HttpGet]
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

        // 批量设置资源状态
        [HttpGet]
        public APIDataResult BatchSetResourceStatus(string resourceIds, int status)
        {
            try {
                int count = service.BatchSetResourceStatus(resourceIds, status);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // 删除指定资源和Group的关系
        [HttpGet]
        public APIDataResult DeleteResourceGroupByResourceIds(string resourceIds)
        {
            try {
                int count = service.DeleteResourceGroupByResourceIds(resourceIds);
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
        [HttpGet]
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
        [HttpGet]
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
        [HttpGet]
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
        [HttpGet]
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
        [HttpGet]
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
        [HttpGet]
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

        // 获取某个资源来源下的资源数量
        [HttpGet]
        public APIDataResult GetResourceCountBySourceFrom(int orgId, int sourceId)
        {
            try {
                int count = service.GetResourceCountBySourceFrom(orgId, sourceId);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // 设置某个来源下的所有资源的SourceFrom为空
        [HttpGet]
        public APIDataResult ClearSourceFrom(int sourceId)
        {
            try {
                int count = service.ClearSourceFrom(sourceId);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // 获取该用户下的资源个数
        [HttpGet]
        public APIDataResult GetResourceCountByUserId(int userId)
        {
            try {
                int count = service.GetResourceCountByUserId(userId);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // 划分某一用户的资源到另一用户名下
        [HttpGet]
        public APIDataResult ChangeResourceUserOwner(int sourceUserId, int targetUserId)
        {
            try {
                int count = service.ChangeResourceUserOwner(sourceUserId, targetUserId);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // 划分某一用户的资源到另一用户所属组下
        [HttpGet]
        public APIDataResult ChangeResourceGroupOwner(int sourceUserId, int targetGroupId)
        {
            try {
                int count = service.ChangeResourceGroupOwner(sourceUserId, targetGroupId);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult GetResourcesByUserId(int userId)
        {
            try {
                List<int> resourceIds = service.GetResourcesByUserId(userId);
                APIDataResult dataResult = new APIDataResult(200, resourceIds);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }


        /// <summary>
        /// 将用户的资源划入公库
        /// </summary>
        /// <param name="userId"></param>
        [HttpGet]
        public APIDataResult AssignUserResourcesToPublic(int userId)
        {
            try {
                service.AssignUserResourcesToPublic(userId);
                APIDataResult dataResult = new APIDataResult(200, string.Empty);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        /// <summary>
        /// 将用户的资源划入未分配
        /// </summary>
        /// <param name="userId"></param>
        [HttpGet]
        public APIDataResult AssignUserResourcesToUnassigned(int userId)
        {
            try {
                service.AssignUserResourcesToUnassigned(userId);
                APIDataResult dataResult = new APIDataResult(200, string.Empty);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // 获取资源属于哪个项目
        [HttpGet]
        public APIDataResult GetResourceProject(int resourceId)
        {
            try {
                var entity = service.GetResourceProject(resourceId);
                APIDataResult dataResult = new APIDataResult(200, entity);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // 获取用户洽谈中的资源总条数(洽谈中的)
        [HttpGet]
        public APIDataResult GetTalkingResourceCountByUserId(int userId)
        {
            try {
                int count = service.GetTalkingResourceCountByUserId(userId);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // 获取项目下所有资源Id
        [HttpGet]
        public APIDataResult GetProjectResourceIds(int projectId)
        {
            try {
                IEnumerable<int> resourceIds = service.GetProjectResourceIds(projectId);
                APIDataResult dataResult = new APIDataResult(200, resourceIds);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // 批量获取ResourceGroup
        [HttpGet]
        public APIDataResult GetResourceGroupsByResourceIds(string resourceIds)
        {
            if (string.IsNullOrEmpty(resourceIds)) {
                return new APIDataResult(-1, "", "参数resourceIds不能为空");
            }
            try {
                IEnumerable<Entity.ResourceGroup> resourceGroups = service.GetResourceGroupsByResourceIds(resourceIds);
                APIDataResult dataResult = new APIDataResult(200, resourceGroups);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        // 批量获取ResourceUser
        [HttpGet]
        public APIDataResult GetResourceUsersByResourceIds(string resourceIds)
        {
            if (string.IsNullOrEmpty(resourceIds)) {
                return new APIDataResult(-1, "", "参数resourceIds不能为空");
            }
            try {
                IEnumerable<Entity.ResourceUser> resourceGroups = service.GetResourceUsersByResourceIds(resourceIds);
                APIDataResult dataResult = new APIDataResult(200, resourceGroups);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        /// <summary>
        /// 资源批量分配
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public APIDataResult ResourceBatchAssign(string resourceIds, int groupId, int userId, int operatorId)
        {
            try {
                bool result = service.ResourceBatchAssign(resourceIds, groupId, userId, operatorId);
                APIDataResult dataResult = new APIDataResult(200, result);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, "", ex.Message);
                return dataResult;
            }
        }

        // 根据ResourceId集合批量删除ResourceUser
        [HttpGet]
        public APIDataResult BatchDeleteResourceUser(string resourceIds)
        {
            try {
                int count = service.BatchDeleteResourceUser(resourceIds);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, "", ex.Message);
                return dataResult;
            }
        }

        // 将某一资源划分给自己
        [HttpGet]
        public APIDataResult DivideToMe(int resourceId, int groupId, int userId)
        {
            try {
                bool result = service.DivideToMe(resourceId, groupId, userId);
                APIDataResult dataResult = new APIDataResult(200, result);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        /// <summary>
        /// 批量更新ResourceProject(一般用于将一批资源从一个项目迁移到另一个项目下，这种操作比较少见)
        /// </summary>
        /// <param name="resourceIds"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet]
        public APIDataResult UpdateResourceProjectByResourceIds(string resourceIds, int projectId)
        {
            try {
                int count = service.UpdateResourceProjectByResourceIds(resourceIds, projectId);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        /// <summary>
        /// 批量更新ResourceGroup(一般用于将一个用户从一个组划分到另外一个组，此时他的资源应同时迁入该组)
        /// </summary>
        /// <param name="resourceIds"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpGet]
        public APIDataResult UpdateResourceGroupByResourceIds(string resourceIds, int groupId)
        {
            try {
                int count = service.UpdateResourceGroupByResourceIds(resourceIds, groupId);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        /// <summary>
        /// 批量删除ResourceProject
        /// </summary>
        /// <param name="resourceIds"></param>
        /// <returns></returns>
        [HttpGet]
        public APIDataResult BatchDeleteResourceProject(string resourceIds)
        {
            try {
                int count = service.BatchDeleteResourceProject(resourceIds);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        /// <summary>
        /// 批量删除ResourceGroup
        /// </summary>
        /// <param name="resourceIds"></param>
        /// <returns></returns>
        [HttpGet]
        public APIDataResult BatchDeleteResourceGroup(string resourceIds)
        {
            try {
                int count = service.BatchDeleteResourceGroup(resourceIds);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }


        /// <summary>
        /// 批量插入Resource
        /// 因不能返回成功插入的实体的主键，该方法暂时没有被使用
        /// </summary>
        /// <param name="resources"></param>
        /// <returns></returns>
        [HttpPost]
        public APIDataResult BatchInsertResource([FromBody]IEnumerable<Entity.ResourceEntity> resources)
        {
            try {
                int count = service.BatchInsertResource(resources);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        /// <summary>
        /// 批量插入ResourceOrganization
        /// </summary>
        /// <param name="resourceOrgs"></param>
        /// <returns></returns>
        [HttpPost]
        public APIDataResult BatchInsertResourceOrg([FromBody]IEnumerable<Entity.ResourceOrganization> resourceOrgs)
        {
            try {
                int count = service.BatchInsertResourceOrg(resourceOrgs);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        /// <summary>
        /// 批量插入ResourceProject
        /// </summary>
        /// <param name="resourceProjects"></param>
        /// <returns></returns>
        [HttpPost]
        public APIDataResult BatchInsertResourceProject([FromBody]IEnumerable<Entity.ResourceProject> resourceProjects)
        {
            try {
                int count = service.BatchInsertResourceProject(resourceProjects);
                APIDataResult dataResult = new APIDataResult(200, count);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        /// <summary>
        /// 批量插入ResourceGroup
        /// </summary>
        /// <param name="resourceGroups"></param>
        /// <returns></returns>
        [HttpPost]
        public APIDataResult ResourceGroupBatchInsert([FromBody]IEnumerable<Entity.ResourceGroup> resourceGroups)
        {
            try {
                bool result = service.ResourceGroupBatchInsert(resourceGroups);
                APIDataResult dataResult = new APIDataResult(200, result);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        /// <summary>
        /// 批量插入ResourceUser
        /// </summary>
        /// <param name="resourceUsers"></param>
        /// <returns></returns>
        [HttpPost]
        public APIDataResult ResourceUserBatchInsert([FromBody]IEnumerable<Entity.ResourceUser> resourceUsers)
        {
            try {
                bool res = service.ResourceUserBatchInsert(resourceUsers);
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