using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Orion.CRM.WebApp.App_Data;
using Orion.CRM.WebTools;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Orion.CRM.WebApp.Controllers
{
    /// <summary>
    /// 基控制器
    /// </summary>
    [TypeFilter(typeof(AppRuntimeFilter))]
    public class BaseController : Controller
    {
        //public BaseController()
        //{

        //}
        ////private readonly AppConfig _appConfig;
        //public BaseController(IOptions<AppConfig> optionsAccessor)
        //{
        //    // 初始化_AppConfig
        //    _AppConfig = optionsAccessor.Value;

        //    // 初始化_AppUser
        //    if (HttpContext.Request.Cookies["user"] != null) {
        //        string encryptUserInfo = HttpContext.Request.Cookies["user"];
        //        string decryptUserInfo = DesEncrypt.Decrypt(encryptUserInfo, _AppConfig.DesEncryptKey);
        //        var appUser = JsonConvert.DeserializeObject<Models.Account.AppUserModel>(decryptUserInfo);

        //        _AppUser = appUser;
        //    }
        //    else {
        //        _AppUser = null;
        //    }

        //}


        public Models.Account.AppUserModel _AppUser
        {
            get
            {
                if (HttpContext.Request.Cookies["user"] != null) {
                    var config = HttpContext.Items["appConfig"] as AppConfig;

                    string encryptUserInfo = HttpContext.Request.Cookies["user"];
                    string decryptUserInfo = DesEncrypt.Decrypt(encryptUserInfo, config.DesEncryptKey);
                    var appUser = JsonConvert.DeserializeObject<Models.Account.AppUserModel>(decryptUserInfo);

                    return appUser;
                }
                else {
                    return null;
                }
            }
            set { _AppUser = value; }
        }

        public AppConfig _AppConfig
        {
            get
            {
                var config = HttpContext.Items["appConfig"] as AppConfig;
                return config;
            }
            set
            {
                _AppConfig = value;
            }
        }

    }
}