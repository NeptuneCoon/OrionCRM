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

            // 读取用户信息(从cookie中)
            //string encryptUserInfo = filterContext.HttpContext.Request.Cookies["user"];// cookie中加密的用户信息
            //filterContext.HttpContext.Items.Add("objUser", encryptUserInfo);

            // 读取当前菜单(从cookie中)
            //object encryptTopMenuId = filterContext.HttpContext.Request.Cookies["top_mid"];// cookie中加密的主菜单id
            //filterContext.HttpContext.Items.Add("objTopMenuId", encryptTopMenuId);

            // 读取当前菜单(从cookie中)
            //object encryptLeftMenuId = filterContext.HttpContext.Request.Cookies["left_mid"];// cookie中加密的子菜单id
            //filterContext.HttpContext.Items.Add("objLeftMenuId", encryptLeftMenuId);


            //int? topMenuId = filterContext.HttpContext.Session.GetInt32("TopMenuId");
            //filterContext.HttpContext.Items.Add("TopMenuId", topMenuId);

            //int? leftMenuId = filterContext.HttpContext.Session.GetInt32("LeftMenuId");
            //filterContext.HttpContext.Items.Add("LeftMenuId", leftMenuId);

            //base.OnActionExecuting(filterContext);
        }
    }
}
