using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.ConsoleApp.Models.Menu
{
    public class MenuModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }
        /// <summary>
        /// 菜单图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 菜单指向的URL(二级菜单的URL不为空)
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int SortNo { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 父级菜单
        /// </summary>
        public int? Parent { get; set; }
    }
}
