using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Project
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public int OrgId { get; set; }
        public string ProjectName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
