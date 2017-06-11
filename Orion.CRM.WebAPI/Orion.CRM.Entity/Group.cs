using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    /// <summary>
    /// Group实体类
    /// </summary>
    public class Group
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public int ProjectId { get; set; }
        /// <summary>
        /// 组长
        /// </summary>
        public int ManagerId { get; set; }
    }
}

