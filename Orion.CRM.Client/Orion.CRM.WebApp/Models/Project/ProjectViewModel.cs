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
        //public int CreateUserId { get; set; }
        //public string CreateUserName { get; set; }
    }
}
