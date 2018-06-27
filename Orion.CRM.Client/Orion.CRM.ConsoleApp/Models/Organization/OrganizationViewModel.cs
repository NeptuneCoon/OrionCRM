using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.ConsoleApp.Models.Organization
{
    public class OrganizationViewModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 组织机构名称
        /// </summary>
        public string OrgName { get; set; }
        /// <summary>
        /// 组织机构唯一代码(GUID)
        /// </summary>
        public string OrgCode { get; set; }
        /// <summary>
        /// 组织机构类型，1=公司，2=非公司
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        public bool DeleteFlag { get; set; }
    }
}
