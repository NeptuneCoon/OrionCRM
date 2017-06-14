using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Orion.CRM.WebTools;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Enyim.Caching;
using System.Collections;
using Orion.CRM.WebApp.Models.Account;

namespace Orion.CRM.WebApp.App_Data
{
    /// <summary>
    /// web App运行环境Filter
    /// 此Filter将会读取应用程序配置Appconfig，并将之传递给BaseController(暂时没有找到方法直接在BaseController中直接获取配置)
    /// </summary>
    public class AppRuntimeFilter : ActionFilterAttribute
    {
        private readonly AppConfig _appConfig;
        public AppRuntimeFilter(IOptions<AppConfig> optionsAccessor)
        {
            _appConfig = optionsAccessor.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // 读取配置文件
            filterContext.HttpContext.Items.Add("appConfig", _appConfig);
        }
    }
}
