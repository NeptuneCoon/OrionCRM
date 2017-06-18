using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Source
{
    /// <summary>
    /// 资源来源
    /// </summary>
    public class ResourceSource
    {
        public int Id { get; set; }
        public string SourceName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public int OrgId { get; set; }
    }
}
