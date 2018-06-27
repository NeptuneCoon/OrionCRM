using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    /// <summary>
    /// UserRole实体类
    /// </summary>
    public class UserRole
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}

