using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    public class ResourceTag
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public int ResourceId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
