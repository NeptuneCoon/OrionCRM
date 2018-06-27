using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    public class DataPermissionCategory
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 该数据权限类别下包括的数据权限
        /// </summary>
        public List<DataPermission> PermissionList { get; set; }
    }
}
