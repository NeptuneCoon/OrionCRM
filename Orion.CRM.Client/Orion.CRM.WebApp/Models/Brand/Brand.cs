using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Brand
{
    public class Brand
    {
        public int Id { get; set; }
        public string BrandName { get; set; }
        public string Description { get; set; }
        public int ProjectId { get; set; }

        // 扩展属性
        public string ProjectName { get; set; }
        public int CustomerCount { get; set; }//该品牌的客户数
    }
}
