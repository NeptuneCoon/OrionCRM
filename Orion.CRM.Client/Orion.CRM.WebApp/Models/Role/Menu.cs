using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Role
{
    /// <summary>
    /// 简化的菜单模型
    /// </summary>
    public class Menu
    {
        public int Id { get; set; }
        public string Icon { get; set; }
        public string MenuName { get; set; }
        public string URL { get; set; }
        public int SortNo { get; set; }

        public int? Parent { get; set; }
    }
}
