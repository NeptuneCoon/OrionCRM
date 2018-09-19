using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Account
{
    public class AppUserModel
    {
        public int Id { get; set; }
        public int OrgId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RealName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Wechat { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public int Enable { get; set; }
        

        /// <summary>
        /// 外键属性，用户所属项目Id
        /// </summary>
        public int? ProjectId { get; set; }
        /// <summary>
        /// 外键属性，用户所属业务组Id
        /// </summary>
        public int? GroupId { get; set; }
        /// <summary>
        /// 外键属性，用户所属角色Id
        /// </summary>
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
