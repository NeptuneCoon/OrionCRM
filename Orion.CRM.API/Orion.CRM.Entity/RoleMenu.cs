using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    /// <summary>
    /// RoleMenu实体类
    /// </summary>
    public class RoleMenu
    {
        //public int Id { get; set; }//批量插入的实体不能有主键
        public DateTime CreateTime { get; set; }
        public int RoleId { get; set; }
        public int MenuId { get; set; }
    }
}

