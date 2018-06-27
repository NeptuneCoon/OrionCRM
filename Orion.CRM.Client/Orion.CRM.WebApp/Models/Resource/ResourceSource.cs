using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Resource
{
    /// <summary>
    /// 资源来源情况
    /// </summary>
    public class ResourceSource
    {
        /// <summary>
        /// 来源Id
        /// </summary>
        public int SourceId { get; set; }
        /// <summary>
        /// 来源名称
        /// </summary>
        public string SourceName { get; set; }
        /// <summary>
        /// 来源下的资源个数
        /// </summary>
        public int ResourceCount { get; set; }
    }
}
