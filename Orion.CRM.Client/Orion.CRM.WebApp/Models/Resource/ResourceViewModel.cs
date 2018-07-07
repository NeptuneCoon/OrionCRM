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
        public string Tel { get; set; }
        public string Wechat { get; set; }
        public string QQ { get; set; }
        public string Email { get; set; }
        public string Province { get; set; } // 省或直辖市
        public string City { get; set; } // 城市
        public string Address { get; set; } // 街道地址
        public int? Status { get; set; }
        public int? SourceFrom { get; set; }
        public int? Inclination { get; set; }

        //public string StatusText { get; set; }
        //public string SourceFromText { get; set; }
        //public string InclinationText { get; set; }

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
        public List<Models.Source.Source> Sources { get; set; }
        /// <summary>
        /// 意向群
        /// </summary>
        public List<App_Data.SelectItem> Inclinations { get; set; }
        /// <summary>
        /// 资源归属(1=本人，0=未分配)
        /// </summary>
        public int ResourceBelong { get; set; }
    }
}
