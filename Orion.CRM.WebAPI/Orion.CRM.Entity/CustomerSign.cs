using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    /// <summary>
    /// Resource实体类
    /// </summary>
    public class CustomerSign
    {
        public int Id { get; set; }
        public int ResourceId { get; set; }
        public DateTime SignTime { get; set; }
        public int SignUserId { get; set; }
        public int Amount { get; set; }
        public DateTime CreateTime { get; set; }
        public int AppendUserId { get; set; }

        // 扩展属性
        /// <summary>
        /// 客户姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 促成此次签约的业务人员
        /// </summary>
        public string SignMan { get; set; }
        /// <summary>
        /// 此条记录的添加人
        /// </summary>
        public string AppendMan { get; set; }
    }
}
