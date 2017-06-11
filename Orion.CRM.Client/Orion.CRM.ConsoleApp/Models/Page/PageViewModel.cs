using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.ConsoleApp.Models.Page
{
    public class PageViewModel
    {
        /// <summary>
        /// Page主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 页面名称
        /// </summary>
        public string PageName { get; set; }
        /// <summary>
        /// 页面URL(相对路径)
        /// </summary>
        public string PageURL { get; set; }
        /// <summary>
        /// 所属菜单Id
        /// </summary>
        public int MenuId { get; set; }
        /// <summary>
        /// 所属菜单名称
        /// </summary>
        public string MenuName { get; set; }
        /// <summary>
        /// 是否是菜单的默认打开页面，0=否，1=是
        /// </summary>
        public int DefaultPage { get; set; }

        /// <summary>
        /// 所有二级菜单(页面需要指定其所属的二级菜单)
        /// </summary>
        public List<Models.Menu.MenuModel> Level2Menus { get; set; }
    }
}
