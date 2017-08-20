using Orion.CRM.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Orion.CRM.DataAccess
{
    public class ResourceTagDataAdapter : DataAdapter
    {
        public int InsertResourceTag(Entity.ResourceTag resourceTag)
        {
            if (resourceTag == null) return -1;

            SqlParameter[] paramArr = {
                new SqlParameter("@TagId", resourceTag.TagId),
                new SqlParameter("@ResourceId", resourceTag.ResourceId),
                new SqlParameter("@CreateTime", resourceTag.CreateTime)
            };

            int identityId = SqlMapHelper.ExecuteSqlMapScalar<int>("ResourceTag", "InsertResourceTag", paramArr);
            return identityId;
        }

        public bool DeleteResourceTag(int resourceId)
        {
            if (resourceId <= 0) return false;

            SqlParameter param = new SqlParameter("@ResourceId", resourceId);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("ResourceTag", "DeleteResourceTag", param);

            return count > 0;
        }

        // 批量删除资源和标签的关系
        public int BatchDeleteResourceTag(string resourceIds)
        {
            if (string.IsNullOrEmpty(resourceIds)) return 0;

            SqlMapDetail mapDetail = (SqlMapDetail)SqlMapFactory.GetSqlMapDetail("ResourceTag", "BatchDeleteResourceTag").Clone();
            mapDetail.OriginalSqlString = mapDetail.OriginalSqlString.Replace("@ResourceId", resourceIds);

            int count = SqlMapHelper.ExecuteSqlMapNonQuery(mapDetail);
            return count;
        }

        public bool ResourceTagBatchInsert(IEnumerable<Entity.ResourceTagBatchInsert> resourceTags)
        {
            bool result = SqlMapHelper.ExecuteBatchInsert<Entity.ResourceTagBatchInsert>("ResourceTag", "ResourceTagBatchInsert", resourceTags);
            return result;
        }

    }
}
