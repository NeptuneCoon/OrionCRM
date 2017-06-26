using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    /// <summary>
    /// TalkRecord实体类
    /// </summary>
    public class TalkRecord
    {
        public int Id { get; set; }
        public int ResourceId { get; set; }
        public int TalkWay { get; set; }
        public string TalkResult { get; set; }
        public int UserId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
