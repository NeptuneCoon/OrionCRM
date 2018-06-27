using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    /// <summary>
    /// 角色和菜单复合模型
    /// </summary>
    public class RoleMenuComplex
    {
        public int RoleId { get; set; }
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public string Icon { get; set; }
        public string URL { get; set; }
        public int SortNo { get; set; }
        public int? Parent { get; set; }
    }
}
