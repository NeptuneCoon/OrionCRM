using Orion.CRM.Core;
using Orion.CRM.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Orion.CRM.DataAccess
{
    public class DataPermissionDataAdapter : DataAdapter
    {
        public IEnumerable<DataPermissionComplex> GetDataPermissionCategories()
        {
            var categories = SqlMapHelper.GetSqlMapResult<DataPermissionComplex>("DataPermissionDomain", "GetDataPermissionCategories");
            return categories;
        }

        public IEnumerable<RoleDataPermission> GetRoleDataPermissions(int roleId)
        {
            SqlParameter param = new SqlParameter("@RoleId", roleId);
            var rolePermissions = SqlMapHelper.GetSqlMapResult<RoleDataPermission>("DataPermissionDomain", "GetRoleDataPermissions", param);
            return rolePermissions;
        }

        public int DeleteRoleDataPermissions(int roleId)
        {
            SqlParameter param = new SqlParameter("@RoleId", roleId);
            int count = SqlMapHelper.ExecuteSqlMapNonQuery("DataPermissionDomain", "DeleteRoleDataPermissions", param);
            return count;
        }

        //public int InsertRoleDataPermission(RoleDataPermission rolePermission)
        //{
        //    SqlParameter[] paramArr = {
        //        new SqlParameter("@RoleId", rolePermission.RoleId),
        //        new SqlParameter("@PermissionId", rolePermission.PermissionId),
        //        new SqlParameter("@CreateTime", rolePermission.CreateTime)
        //    };

        //    int identityId = SqlMapHelper.ExecuteSqlMapScalar<int>("DataPermissionDomain", "InsertRoleDataPermission", paramArr);
        //    return identityId;
        //}

        public bool RoleDataPermissionBatchInsert(IEnumerable<Entity.RoleDataPermission> rolePermissions)
        {
            bool result = SqlMapHelper.ExecuteBatchInsert<Entity.RoleDataPermission>("DataPermissionDomain", "RoleDataPermissionBatchInsert", rolePermissions);
            return result;
        }
    }
}
