﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Role
{
    public class RoleDataPermission
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
