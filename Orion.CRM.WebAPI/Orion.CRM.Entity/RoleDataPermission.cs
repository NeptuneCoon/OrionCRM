﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    public class RoleDataPermission
    {
        //public int Id { get; set; }//批量插入的实体不能有主键
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
