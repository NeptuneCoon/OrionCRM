using Orion.CRM.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Orion.CRM.DataAccess
{
    public class ResourceSourceAdapter : DataAdapter
    {
        public int InsertSource(Entity.ResourceSource source)
        {
            if (source == null) return -1;

            SqlParameter[] paramArr = {
                new SqlParameter("@SourceName", source.SourceName),
                new SqlParameter("@CreateTime", source.CreateTime),
                new SqlParameter("@UpdateTime", source.UpdateTime),
                new SqlParameter("@OrgId",source.OrgId),
            };

            int identityId = SqlMapHelper.ExecuteSqlMapScalar<int>("ResourceSource", "InsertSource", paramArr);
            return identityId;
        }

        public bool UpdateSource(Entity.ResourceSource source)
        {
            if (source == null || source.Id <= 0) return false;
            SqlParameter[] paramArr = {
                new SqlParameter("@Id", source.Id),
                new SqlParameter("@SourceName", source.SourceName),
                new SqlParameter("@UpdateTime", source.UpdateTime),
                new SqlParameter("@OrgId",source.OrgId),
            };

            int count = SqlMapHelper.ExecuteSqlMapNonQuery("ResourceSource", "UpdateSource", paramArr);
            return count > 0;
        }

        public bool DeleteSource(int id)
        {
            if (id <= 0) return false;

            SqlParameter param = new SqlParameter("@Id", id);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("ResourceSource", "DeleteSource", param);

            return count > 0;
        }

        public IEnumerable<Entity.ResourceSource> GetSourcesByOrgId(int orgId)
        {
            SqlParameter param = new SqlParameter("@OrgId", orgId);
            var sources = SqlMapHelper.GetSqlMapResult<Entity.ResourceSource>("ResourceSource", "GetSourcesByOrgId", param);

            return sources;
        }
    }
}
