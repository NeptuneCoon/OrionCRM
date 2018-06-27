using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    /// <summary>
    /// UserProject实体类
    /// </summary>
    public class UserProject
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }
    }
}

