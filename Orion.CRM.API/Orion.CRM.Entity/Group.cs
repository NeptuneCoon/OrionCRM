﻿using System;
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
        public int OrgId { get; set; }
        public string GroupName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public int ProjectId { get; set; }
        /// <summary>
        /// 组长
        /// </summary>
        public int? ManagerId { get; set; }
        /// <summary>
        /// 扩展属性：组长真实姓名
        /// </summary>
        public string ManagerName { get; set; }
        /// <summary>
        /// 扩展属性：所属项目
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 扩展属性：当前组人数
        /// </summary>
        public int UserCount { get; set; }
    }
}

