using Orion.CRM.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Application
{
    public class ResourceTagAppService
    {
        private ResourceTagDataAdapter adapter = new ResourceTagDataAdapter();
        public int InsertResourceTag(Entity.ResourceTag resourceTag)
        {
            return adapter.InsertResourceTag(resourceTag);
        }

        public bool DeleteResourceTag(int resourceId)
        {
            return adapter.DeleteResourceTag(resourceId);
        }

        // 批量删除资源和标签的关系
        public int BatchDeleteResourceTag(string resourceIds)
        {
            return adapter.BatchDeleteResourceTag(resourceIds);
        }

        public bool ResourceTagBatchInsert(IEnumerable<Entity.ResourceTag> resourceTags)
        {
            return adapter.ResourceTagBatchInsert(resourceTags);
        }
    }
}
