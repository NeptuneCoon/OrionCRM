using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Resource
{
    public class ResourceViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public int ProjectId { get; set; }
        public string Mobile { get; set; }
        public string Wechat { get; set; }
        public string QQ { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int? SourceFrom { get; set; }
        public int? Inclination { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 留言内容
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 项目列表
        /// </summary>
        public List<Models.Project.Project> Projects { get; set; }
        
        /// <summary>
        /// 资源来源
        /// </summary>
        public List<Models.Source.ResourceSource> Sources { get; set; }

        /// <summary>
        /// 意向群
        /// </summary>
        public List<App_Data.ResourceInclination> Inclinations { get; set; }
    }
}
