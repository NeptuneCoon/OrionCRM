using Orion.CRM.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Application
{
    public class ResourceAppService
    {
        private ResourceDataAdapter adapter = new ResourceDataAdapter();
        public int InsertResource(Entity.Resource resource)
        {
            return adapter.InsertResource(resource);
        }

        public bool UpdateResource(Entity.Resource resource)
        {
            return adapter.UpdateResource(resource);
        }

        // 删除一条资源
        public bool DeleteResource(int id)
        {
            return adapter.DeleteResource(id);
        }

        public int DeleteResourceUserByUserId(int userId)
        {
            return adapter.DeleteResourceUserByUserId(userId);
        }

        // 恢复一条资源
        public bool RestoreResource(int id)
        {
            return adapter.RestoreResource(id);
        }

        public int InsertResourceOrganization(Entity.ResourceOrganization resourceOrg)
        {
            return adapter.InsertResourceOrganization(resourceOrg);
        }

        public int InsertResourceProject(Entity.ResourceProject resourceProject)
        {
            return adapter.InsertResourceProject(resourceProject);
        }

        /// <summary>
        /// 根据筛选条件查询资源
        /// </summary>
        /// <param name="param">筛选条件对象</param>
        /// <returns></returns>
        public IEnumerable<Entity.Resource> GetResourcesByCondition(Entity.ResourceSearchParams param)
        {
            return adapter.GetResourcesByCondition(param);
        }

        // 根据Id获取资源
        public Entity.Resource GetResourceById(int id)
        {
            return adapter.GetResourceById(id);
        }

        /// <summary>
        /// 根据筛选条件获取符合条件的资源个数
        /// </summary>
        /// <param name="param">筛选条件对象</param>
        /// <returns></returns>
        public int GetResourceCountByCondition(Entity.ResourceSearchParams param)
        {
            return adapter.GetResourceCountByCondition(param);
        }

        /// <summary>
        /// 根据姓名/电话(Mobile/Tel)/微信/QQ查询一条资源
        /// </summary>
        /// <param name="key">查询关键词</param>
        /// <param name="orgId">组织机构Id</param>
        /// <returns></returns>
        public Entity.Resource GetResourceByNameMobileWechatQQ(string key, int orgId)
        {
            return adapter.GetResourceByNameMobileWechatQQ(key, orgId);
        }

        // 判断资源是否存在
        public bool IsResourceExist(int orgId, string mobile, string tel, string qq, string wechat)
        {
            return adapter.IsResourceExist(orgId, mobile, tel, qq, wechat);
        }

        // 设置资源状态
        public bool SetResourceStatus(int resourceId, int status)
        {
            return adapter.SetResourceStatus(resourceId, status);
        }

        // 批量设置资源状态
        public int BatchSetResourceStatus(string resourceIds, int status)
        {
            return adapter.BatchSetResourceStatus(resourceIds, status);
        }

        // 删除指定资源和Group的关系
        public int DeleteResourceGroupByResourceIds(string resourceIds)
        {
            return adapter.DeleteResourceGroupByResourceIds(resourceIds);
        }

        // 添加资源和业务组之间的关系
        public int InsertResourceGroup(Entity.ResourceGroup resourceGroup)
        {
            return adapter.InsertResourceGroup(resourceGroup);
        }

        // 修改资源和业务组之间的关系
        public bool UpdateResourceGroup(Entity.ResourceGroup resourceGroup)
        {
            return adapter.UpdateResourceGroup(resourceGroup);
        }

        // 获取资源和业务组之间的关系
        public Entity.ResourceGroup GetResourceGroup(int resourceId)
        {
            return adapter.GetResourceGroup(resourceId);
        }

        // 添加资源和用户之间的关系
        public int InsertResourceUser(Entity.ResourceUser resourceUser)
        {
            return adapter.InsertResourceUser(resourceUser);
        }

        // 修改资源和用户之间的关系
        public bool UpdateResourceUser(Entity.ResourceUser resourceUser)
        {
            return adapter.UpdateResourceUser(resourceUser);
        }

        // 获取资源和用户之间的关系
        public Entity.ResourceUser GetResourceUser(int resourceId)
        {
            return adapter.GetResourceUser(resourceId);
        }

        // 获取未分配至业务组的资源
        public IEnumerable<Entity.UnassignedResource> GetGroupUnAssignedResources(int projectId)
        {
            return adapter.GetGroupUnAssignedResources(projectId);
        }

        // 获取未分配至业务组的资源个数
        public int GetGroupUnAssignedResourceCount(int projectId)
        {
            return adapter.GetGroupUnAssignedResourceCount(projectId);
        }

        // 获取未分配至业务员的资源个数
        public int GetUserUnAssignedResourceCount(int orgId)
        {
            return adapter.GetUserUnAssignedResourceCount(orgId);
        }

        // 获取资源来源情况
        public IEnumerable<Entity.ResourceSource> GetResourceSourceStat(int orgId)
        {
            return adapter.GetResourceSourceStat(orgId);
        }

        public int GetResourceCountBySourceFrom(int orgId, int sourceId)
        {
            return adapter.GetResourceCountBySourceFrom(orgId, sourceId);
        }

        public int ClearSourceFrom(int sourceId)
        {
            return adapter.ClearSourceFrom(sourceId);
        }

        // 获取该用户下的资源个数
        public int GetResourceCountByUserId(int userId)
        {
            return adapter.GetResourceCountByUserId(userId);
        }

        // 划分某一用户的资源到另一用户名下
        public int ChangeResourceUserOwner(int sourceUserId, int targetUserId)
        {
            return adapter.ChangeResourceUserOwner(sourceUserId, targetUserId);
        }

        // 划分某一用户的资源到另一用户所属组下
        public int ChangeResourceGroupOwner(int sourceUserId, int targetGroupId)
        {
            return adapter.ChangeResourceGroupOwner(sourceUserId, targetGroupId);
        }

        public bool ResourceGroupBatchInsert(IEnumerable<Entity.ResourceGroup> resourceGroups)
        {
            return adapter.ResourceGroupBatchInsert(resourceGroups);
        }

        public List<int> GetResourcesByUserId(int userId)
        {
            return adapter.GetResourcesByUserId(userId);
        }

        /// <summary>
        /// 将用户的资源划入公库
        /// </summary>
        /// <param name="userId"></param>
        public void AssignUserResourcesToPublic(int userId)
        {
            adapter.AssignUserResourcesToPublic(userId);
        }

        /// <summary>
        /// 将用户的资源划入未分配
        /// </summary>
        /// <param name="userId"></param>
        public void AssignUserResourcesToUnassigned(int userId)
        {
            adapter.AssignUserResourcesToUnassigned(userId);
        }

        // 获取资源属于哪个项目
        public Entity.ResourceProject GetResourceProject(int resourceId)
        {
            return adapter.GetResourceProject(resourceId);
        }

        public int GetTalkingResourceCountByUserId(int userId)
        {
            return adapter.GetTalkingResourceCountByUserId(userId);
        }

        // 获取项目下所有资源Id
        public IEnumerable<int> GetProjectResourceIds(int projectId)
        {
            return adapter.GetProjectResourceIds(projectId);
        }

        // 根据ResourceId集合批量删除ResourceUser
        public int BatchDeleteResourceUser(string resourceIds)
        {
            return adapter.BatchDeleteResourceUser(resourceIds);
        }

        /// <summary>
        /// 资源批量分配
        /// </summary>
        /// <returns></returns>
        public bool ResourceBatchAssign(string resourceIds, int groupId, int userId)
        {
            // 1.删除和这些资源有关的ResourceGroup:delete from [ResourceGroup] where ResourceId in(@ResourceIds)
            int count1 = adapter.BatchDeleteResourceGroup(resourceIds);
            // 2.删除和这些资源有关的ResourceUser:delete from [ResourceUser] where ResourceId in(@ResourceIds)
            int count2 = adapter.BatchDeleteResourceUser(resourceIds);
            // 3.重新插入这些资源和Group的关系
            // 4.重新插入这些资源和User的关系
            string[] resourceIdArr = resourceIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (resourceIdArr != null && resourceIdArr.Length > 0) {
                foreach (var resourceIdStr in resourceIdArr) {
                    int resourceId = Convert.ToInt32(resourceIdStr);
                    adapter.InsertResourceGroup(new Entity.ResourceGroup() { CreateTime = DateTime.Now, GroupId = groupId, ResourceId = resourceId });
                    adapter.InsertResourceUser(new Entity.ResourceUser() { CreateTime = DateTime.Now, UserId = userId, ResourceId = resourceId });
                }
            }
            return true;
        }
    }
}
