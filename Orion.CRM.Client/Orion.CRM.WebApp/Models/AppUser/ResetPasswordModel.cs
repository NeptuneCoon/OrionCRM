using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.AppUser
{
    public class ResetPasswordModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Realname { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
