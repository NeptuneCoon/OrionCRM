using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    /// <summary>
    /// SystemPage实体类
    /// </summary>
    public class SystemPage
    {
        public int Id { get; set; }
        public string PageName { get; set; }
        public string PageURL { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public int DefaultPage { get; set; }
        public int MenuId { get; set; }
        public string MenuName { get; set; }
    }
}

