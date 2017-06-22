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
        public List<Models.Source.ResourceSource> SourceList { get; set; } 
        public List<App_Data.SelectItem> TalkCountList { get; set; }
        #endregion

        /// <summary>
        /// 查询到的当页资源列表数据
        /// </summary>
        public List<Models.Resource.Resource> Resources { get; set; }
    }
}
