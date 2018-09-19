using Orion.CRM.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Orion.CRM.DataAccess
{
    public class CRMLogDataAdapter : DataAdapter
    {
        public long InsertActionLog(Entity.CRMLog.ActionLog log)
        {
            if (log == null) return -1;

            SqlParameter[] paramArr = {
                new SqlParameter("@PageURL", log.PageURL),
                new SqlParameter("@QueryString", CheckNull(log.QueryString)),
                new SqlParameter("@UserId", log.UserId),
                new SqlParameter("@UserName", log.UserName),
                new SqlParameter("@RealName", log.RealName),
                new SqlParameter("@RoleId", log.RoleId),
                new SqlParameter("@RoleName", log.RoleName)
            };

            long identityId = SqlMapHelper.ExecuteSqlMapScalar<long>("CRMLog", "InsertActionLog", paramArr);
            return identityId;
        }

        public long InsertErrorLog(Entity.CRMLog.ErrorLog log)
        {
            if (log == null) return -1;

            SqlParameter[] paramArr = {
                new SqlParameter("@Origin", log.Origin),
                new SqlParameter("@ClassName", log.ClassName),
                new SqlParameter("@MethodName", log.MethodName),
                new SqlParameter("@Parameters", CheckNull(log.Parameters)),
                new SqlParameter("@ErrorMsg", log.ErrorMsg)
            };

            long identityId = SqlMapHelper.ExecuteSqlMapScalar<long>("CRMLog", "InsertErrorLog", paramArr);
            return identityId;
        }

        public long InsertLoginLog(Entity.CRMLog.LoginLog log)
        {
            if (log == null) return -1;

            SqlParameter[] paramArr = {
                new SqlParameter("@UserId", log.UserId),
                new SqlParameter("@UserName", log.UserName),
                new SqlParameter("@RealName", log.RealName),
                new SqlParameter("@IP", log.IP)
            };

            long identityId = SqlMapHelper.ExecuteSqlMapScalar<long>("CRMLog", "InsertLoginLog", paramArr);
            return identityId;
        }
    }
}
