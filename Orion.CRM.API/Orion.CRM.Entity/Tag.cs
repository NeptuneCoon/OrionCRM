using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    public class Tag
    {
        public int Id { get; set; }
        public string TagName { get; set; }
        public int UserId { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
