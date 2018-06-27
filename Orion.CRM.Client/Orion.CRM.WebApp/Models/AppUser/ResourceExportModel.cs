using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.AppUser
{
    public class ResourceExportModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
        /// <summary>
        /// 洽谈中的资源数量
        /// </summary>
        public int ResourceCount { get; set; }

        /// <summary>
        /// 导出方向
        /// </summary>
        public string ExportDirection { get; set; } 
        /// <summary>
        /// 导出具体目标
        /// </summary>
        public string ExportTarget { get; set; }


        public IEnumerable<Group.Group> GroupList { get; set; }
    }
}
