using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    public class RoleDataPermission
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public int PermissionValue { get; set; }
    }
}
