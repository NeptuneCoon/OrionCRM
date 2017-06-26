using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Account
{
    public class PasswordModel
    {
        public int UserId { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
