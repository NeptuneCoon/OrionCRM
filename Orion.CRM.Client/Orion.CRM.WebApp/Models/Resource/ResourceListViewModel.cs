using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Resource
{
    public class ResourceListViewModel
    {
        #region 查询参数
        //public string name { get; set; }
        //public string key { get; set; }
        //public int? pid { get; set; }
        //public int? gid { get; set; }
        //public int? uid { get; set; }
        //public int? status { get; set; }
        //public int? talk { get; set; }
        //public int? inc { get; set; }
        //public int? source { get; set; }
        //public int? assign { get; set; } 
        #endregion

        public ResourceSearchParams Params { get; set; }

        // 当前用户所属的ProjectId
        public int? ProjectId { get; set; }

        #region 视图展示性数据
        public List<Models.Project.Project> ProjectList { get; set; }
        /// <summary>
        /// 业务组
        /// </summary>
        public List<Models.Group.Group> GroupList { get; set; }
        /// <summary>
        /// 业务员
        /// </summary>
        public List<Models.AppUser.AppUserViewModel> SalerList { get; set; }
        public List<App_Data.SelectItem> StatusList { get; set; }
        public List<App_Data.SelectItem> InclinationList { get; set; }
        public List<Models.Source.Source> SourceList { get; set; } 
        public List<App_Data.SelectItem> TalkCountList { get; set; }

        /// <summary>
        /// 资源可见范围<>(4=公司资源，3=本项目资源，2=本组资源，1=本人资源)
        /// </summary> 
        public int RoleResourceVisible { get; set; }
        /// <summary>
        /// 客户电话是否可见(True=可见，Flase=隐藏)
        /// </summary>
        public bool RoleResourcePhoneVisible { get; set; }

        /// <summary>
        /// 资源操作权限
        /// </summary>
        public List<Models.Role.RoleDataPermission> RoleResourceHandle { get; set; }

        /// <summary>
        /// 资源查询权限
        /// </summary>
        public bool CanSearch { get; set; }
        /// <summary>
        /// 资源分配权限
        /// </summary>
        public bool CanAssign { get; set; }
        /// <summary>
        /// 资源批量分配权限
        /// </summary>
        public bool CanBatchAssign { get; set; }

        #endregion
        

        /// <summary>
        /// 查询到的当页资源列表数据
        /// </summary>
        public List<Models.Resource.Resource> Resources { get; set; }

        /// <summary>
        /// 用户定义的资源标签
        /// </summary>
        public List<Models.Tag.Tag> Tags { get; set; }
    }
}
