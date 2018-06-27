using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.ConsoleApp.Models.Menu
{
    public class MenuEditViewModel
    {
        /// <summary>
        /// 当前要编辑的菜单的类型(1=一级菜单，2=二级菜单)
        /// </summary>
        public int Type { get; set; }
        
        /// <summary>
        /// 当前创建的菜单对象
        /// </summary>
        public MenuModel Menu { get; set; }

        /// <summary>
        /// 所有父级菜单
        /// </summary>
        public IEnumerable<MenuModel> ParentMenus { get; set; }
    }
}
