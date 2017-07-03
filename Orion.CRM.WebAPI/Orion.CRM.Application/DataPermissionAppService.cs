using Orion.CRM.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Application
{
    public class DataPermissionAppService
    {
        DataPermissionDataAdapter adapter = new DataPermissionDataAdapter();

        public IEnumerable<Entity.DataPermissionComplex> GetDataPermissionCategories()
        {
            return adapter.GetDataPermissionCategories();
        }

        public IEnumerable<Entity.RoleDataPermission> GetRoleDataPermissions(int roleId)
        {
            return adapter.GetRoleDataPermissions(roleId);
        }

        public int DeleteRoleDataPermissions(int roleId)
        {
            return adapter.DeleteRoleDataPermissions(roleId);
        }

        //public int InsertRoleDataPermission(Entity.RoleDataPermission rolePermission)
        //{
        //    return adapter.InsertRoleDataPermission(rolePermission);
        //}

        public bool RoleDataPermissionBatchInsert(IEnumerable<Entity.RoleDataPermission> rolePermissions)
        {
            return adapter.RoleDataPermissionBatchInsert(rolePermissions);
        }
    }
}
