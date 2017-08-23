using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Project
{
    /// <summary>
    /// 项目视图模型
    /// </summary>
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public int OrgId { get; set; }
        public string ProjectName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 扩展属性，该项目下的业务组个数
        /// </summary>
        public int GroupCount { get; set; }
        //public int CreateUserId { get; set; }
        //public string CreateUserName { get; set; }
    }
}
