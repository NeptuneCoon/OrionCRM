using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Orion.CRM.WebApp.App_Data
{
    /// <summary>
    /// 匿名过滤器
    /// 如果某些Action不需身份验证，则可以为其添加AnonymousFilter标记以跳过身份验证
    /// </summary>
    public class AnonymousFilter : IFilterMetadata
    {
    }
}
