using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    /// <summary>
    /// 数据权限的类别及子项复合模型
    /// </summary>
    public class DataPermissionComplex
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
    }
}
