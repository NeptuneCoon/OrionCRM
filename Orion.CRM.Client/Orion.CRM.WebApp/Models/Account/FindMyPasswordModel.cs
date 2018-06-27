using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Account
{
    public class FindMyPasswordModel
    {
        public string Email { get; set; }
        public string VerCode { get; set; }
        public string Password { get; set; }
    }
}
