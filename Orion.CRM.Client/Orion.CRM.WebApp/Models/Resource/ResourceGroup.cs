using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.WebApp.Models.Resource
{
    public class ResourceGroup
    {
        //public int Id { get; set; }
        public int ResourceId { get; set; }
        public int GroupId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}

