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

        // 软删除一条资源
        public bool DeleteResource(int id)
        {
            return adapter.DeleteResource(id);
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
    }
}
