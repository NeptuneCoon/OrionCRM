using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Core
{
    public class SqlMap
    {
        /// <summary>
        /// SqlMap的名称，和SqlMap文件名相同，是SqlMap的唯一标识符
        /// </summary>
        public string SqlMapName { get; set; }

        /// <summary>
        /// SqlMap下的SQL语句集合 
        /// </summary>
        public List<SqlMapDetail> SqlMapConfigurations { get; set; }
    }
}
