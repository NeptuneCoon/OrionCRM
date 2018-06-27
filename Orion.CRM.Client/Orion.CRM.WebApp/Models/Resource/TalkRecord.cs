using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Resource
{
    public class TalkRecord
    {
        public int Id { get; set; }
        public int ResourceId { get; set; }
        public int TalkWay { get; set; }
        public string TalkResult { get; set; }
        public int UserId { get; set; }
        public string RealName { get; set; }
        public int Type { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
