using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Resource
{
    public class UnassignedResource
    {
        public int ResourceId { get; set; }
        public int OrgId { get; set; }
        public int ProjectId { get; set; }
    }
}
