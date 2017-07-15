using Orion.CRM.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Orion.CRM.DataAccess
{
    public class CustomerSignDataAdapter
    {
        public int InsertSign(Entity.CustomerSign sign)
        {
            if (sign == null) return -1;

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@ResourceId", sign.ResourceId));
            parameters.Add(new SqlParameter("@SignTime", sign.SignTime));
            parameters.Add(new SqlParameter("@SignUserId", sign.SignUserId));
            parameters.Add(new SqlParameter("@Amount", sign.Amount));
            parameters.Add(new SqlParameter("@CreateTime", sign.CreateTime));
            parameters.Add(new SqlParameter("@AppendUserId", sign.AppendUserId));

            SqlParameter[] paramArr = parameters.ToArray();

            int identityId = SqlMapHelper.ExecuteSqlMapScalar<int>("CustomerSign", "InsertSign", paramArr);
            return identityId;
        }

        public int DeleteSign(int resourceId)
        {
            if (resourceId <= 0) return 0;
            SqlParameter param = new SqlParameter("@ResourceId", resourceId);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("CustomerSign", "DeleteSign", param);
            return count;
        }

        public Entity.CustomerSign GetSignByResourceId(int resourceId)
        {
            if (resourceId <= 0) return null;
            SqlParameter param = new SqlParameter("@ResourceId", resourceId);
            Entity.CustomerSign entity = SqlMapHelper.GetSqlMapSingleResult<Entity.CustomerSign>("CustomerSign", "GetSignByResourceId", param);
            return entity;
        }
    }
}
