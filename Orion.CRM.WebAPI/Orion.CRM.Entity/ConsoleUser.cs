using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    /// <summary>
    /// ConsoleUser实体类
    /// </summary>
    public class ConsoleUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RealName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int Enable { get; set; }
    }
}

