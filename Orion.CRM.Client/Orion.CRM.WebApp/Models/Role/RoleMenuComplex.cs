using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Role
{
    /// <summary>
    /// 角色和菜单复合对象
    /// select A.RoleId,A.MenuId,B.MenuName,B.Icon,B.URL,B.SortNo,B.Parent from RoleMenu A
    /// inner join SystemMenu B on A.MenuId=B.ID
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
