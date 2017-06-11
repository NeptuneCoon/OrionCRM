using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.ConsoleApp.Models.User
{
    public class AppUserViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RealName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Wechat { get; set; }
        public int OrgId { get; set; }
        public string OrgName { get; set; }
        public int Enable { get; set; }

        public IEnumerable<Organization.OrganizationViewModel> OrgList { get; set; }
        public IEnumerable<Role.RoleViewModel> RoleList { get; set; }
    }
}
