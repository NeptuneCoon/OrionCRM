using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.Models.Resource
{
    /// <summary>
    /// 客户查询工具视图模型
    /// </summary>
    public class CustomerSearchViewModel
    {
        // 查询条件
        public string key { get; set; }

        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Mobile { get; set; }
        public string Tel { get; set; }
        public string Wechat { get; set; }
        public string QQ { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Remark { get; set; }
        public string Message { get; set; }
        public string SexText { get; set; }

        public string StatusText { get; set; }
        public string SourceFromText { get; set; }
        public string InclinationText { get; set; }
        public List<Models.Resource.TalkRecord> TalkRecords { get; set; }
        public List<Models.AppUser.AppUserComplex> OrgUsers { get; set; }
        public Models.Sign.CustomerSign Sign { get; set; }

    }
}
