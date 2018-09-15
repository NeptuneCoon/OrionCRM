using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    /// <summary>
    /// AppUser实体类
    /// </summary>
    public class AppUser
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
        public int? IsTalkMan { get; set; }//是否是谈单人(0=不是，1=是)


        #region 扩展属性
        public int? ProjectId { get; set; }
        public int? GroupId { get; set; }
        public int? RoleId { get; set; }
        public string OrgName { get; set; }
        public string ProjectName { get; set; }
        public string RoleName { get; set; }
        public string GroupName { get; set; } 
        #endregion
    }
}

