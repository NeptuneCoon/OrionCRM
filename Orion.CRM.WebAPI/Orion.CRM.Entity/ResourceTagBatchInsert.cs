using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    /// <summary>
    /// 用于批量插入的实体
    /// </summary>
    public class ResourceTagBatchInsert
    {
        public int TagId { get; set; }
        public int ResourceId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
