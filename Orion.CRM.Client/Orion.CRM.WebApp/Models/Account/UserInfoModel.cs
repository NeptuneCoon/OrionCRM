using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Account
{
    public class UserInfoModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Wechat { get; set; }
        public int Enable { get; set; }
        public DateTime UpdateTime { get; set; }
        public int OrgId { get; set; }

        public int? GroupId { get; set; }
        public string GroupName { get; set; }
    }
}
