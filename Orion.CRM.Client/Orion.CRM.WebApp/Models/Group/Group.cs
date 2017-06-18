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
        public int OrgId { get; set; }
        public string GroupName { get; set; }
        public int ProjectId { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 组长，AppUser表的主键
        /// </summary>
        public int? ManagerId { get; set; }
        /// <summary>
        /// 组长真实姓名(扩展属性)
        /// </summary>
        public string ManagerName { get; set; }
        /// <summary>
        /// 所属项目(扩展属性)
        /// </summary>
        public string ProjectName { get; set; }
    }
}
