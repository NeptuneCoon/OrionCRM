using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.AppUser
{
    public class AppUserViewModel
    {
        #region 基本属性
        public int Id { get; set; }
        public int OrgId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RealName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Wechat { get; set; }
        public int Enable { get; set; }
        #endregion

        #region 扩展属性
        public int? ProjectId { get; set; }
        public int? GroupId { get; set; }
        public int? RoleId { get; set; }
        public string OrgName { get; set; }
        public string ProjectName { get; set; }
        public string RoleName { get; set; }
        public string GroupName { get; set; }
        #endregion


        // 视图数据
        public IEnumerable<Role.Role> RoleList { get; set; }
        public IEnumerable<Project.Project> ProjectList { get; set; }
        public IEnumerable<Group.Group> GroupList { get; set; }
    }
}
