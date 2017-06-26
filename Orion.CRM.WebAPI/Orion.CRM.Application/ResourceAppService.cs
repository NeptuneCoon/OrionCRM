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
    }
}
