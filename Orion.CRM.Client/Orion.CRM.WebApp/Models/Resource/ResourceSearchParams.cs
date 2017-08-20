using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Resource
{
    public class ResourceSearchParams
    {
        /// <summary>
        /// pageIndex
        /// </summary>
        public int pi { get; set; }
        /// <summary>
        /// pageSize
        /// </summary>
        public int ps { get; set; }

        public string name { get; set; }
        public string key { get; set; }
        public int? pid { get; set; }
        public int? gid { get; set; }
        public int? uid { get; set; }
        public int? status { get; set; }
        public int? talk { get; set; }
        public int? inc { get; set; }
        public int? source { get; set; }
        public int? assign { get; set; }
        public List<int> tagids { get; set; }
    }
}
