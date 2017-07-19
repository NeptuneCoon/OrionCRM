using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.AppUser
{
    public class AppUserComplex
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
        public int ProjectId { get; set; }
    }
}
