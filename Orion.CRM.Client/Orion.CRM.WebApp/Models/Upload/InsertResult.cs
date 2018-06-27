using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Upload
{
    /// <summary>
    /// 批量导入写库结果
    /// </summary>
    public class InsertResult
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 成功写入的记录数
        /// </summary>
        public int SuccessCount { get; set; }
        /// <summary>
        /// 重复的记录数
        /// </summary>
        public int RepeatCount { get; set; }
        /// <summary>
        /// 插入失败的记录数
        /// </summary>
        public int FailCount { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public List<string> ErrorMsgs = new List<string>() { };
    }
}
