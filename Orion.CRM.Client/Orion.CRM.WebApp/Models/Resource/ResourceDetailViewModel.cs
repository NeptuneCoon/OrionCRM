using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Resource
{
    public class ResourceDetailViewModel
    {
        public int ResourceId { get; set; }
        public string CustomerName { get; set; }
        public string Mobile { get; set; }
        public string QQ { get; set; }
        public string Wechat { get; set; }
        public string Email { get; set; }
        public string SourceFrom { get; set; }
        public int? Sex { get; set; }
        public int? Inclination { get; set; }
        public string Address { get; set; }
        public int? Status { get; set; }
        public string Remark { get; set; }

        public List<Models.Resource.ResourceNote> ResourceNotes { get; set; }
        public List<Models.Resource.TalkRecord> TalkRecords { get; set; }
    }
}
