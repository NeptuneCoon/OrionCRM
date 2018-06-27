using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    /// <summary>
    /// SystemMenu实体类
    /// </summary>
    public class SystemMenu
    {
        public int Id { get; set; }
        public string MenuName { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string URL { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public int SortNo { get; set; }
        public int? Parent { get; set; }
    }
}

