using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    /// <summary>
    /// Project实体类
    /// </summary>
    public class Project
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public int OrgId { get; set; }
        /// <summary>
        /// 扩展属性，该项目下的业务组个数
        /// </summary>
        public int GroupCount { get; set; }
    }
}

