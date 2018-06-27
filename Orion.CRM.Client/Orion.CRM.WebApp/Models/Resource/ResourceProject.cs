using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Resource
{
    public class ResourceProject
    {
        public int Id { get; set; }
        public int ResourceId { get; set; }
        public int ProjectId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
