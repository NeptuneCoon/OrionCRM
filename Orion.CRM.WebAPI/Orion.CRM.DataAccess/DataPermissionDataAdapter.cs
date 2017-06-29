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

        public IEnumerable<DataPermission> GetRoleDataPermissions(int roleId)
        {
            SqlParameter param = new SqlParameter("@RoleId", roleId);
            return null;
        }
    }
}
