using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    /// <summary>
    /// RolePage实体类
    /// </summary>
    public class RolePage
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public int RoleId { get; set; }
        public int PageId { get; set; }
    }
}

