using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Group
{
    /// <summary>
    /// 业务组
    /// </summary>
    public class Group
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public int ProjectId { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 组长，AppUser表的主键
        /// </summary>
        public int ManagerId { get; set; }
    }
}
