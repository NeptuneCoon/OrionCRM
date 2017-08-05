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

        public IEnumerable<Entity.CustomerSign> GetSignsByTime(int orgId, string beginTime, string endTime)
        {
            if (orgId <= 0) return null;
            SqlParameter[] paramArr ={
                new SqlParameter("@OrgId", orgId),
                new SqlParameter("@BeginTime", beginTime),
                new SqlParameter("@EndTime", endTime)
            };
            var result = SqlMapHelper.GetSqlMapResult<Entity.CustomerSign>("CustomerSign", "GetSignsByTime", paramArr);
            return result;
        }

        public IEnumerable<Entity.CustomerSign> GetGroupMemberSigns(int groupId, string beginTime, string endTime)
        {
            if (groupId <= 0) return null;
            SqlParameter[] paramArr ={
                new SqlParameter("@GroupId", groupId),
                new SqlParameter("@BeginTime", beginTime),
                new SqlParameter("@EndTime", endTime)
            };
            var result = SqlMapHelper.GetSqlMapResult<Entity.CustomerSign>("CustomerSign", "GetGroupMemberSigns", paramArr);
            return result;
        }

        public IEnumerable<Entity.CustomerSign> GetProjectGroupSigns(int projectId, string beginTime, string endTime)
        {
            if (projectId <= 0) return null;
            SqlParameter[] paramArr ={
                new SqlParameter("@ProjectId", projectId),
                new SqlParameter("@BeginTime", beginTime),
                new SqlParameter("@EndTime", endTime)
            };
            var result = SqlMapHelper.GetSqlMapResult<Entity.CustomerSign>("CustomerSign", "GetProjectGroupSigns", paramArr);
            return result;
        }
    }
}
