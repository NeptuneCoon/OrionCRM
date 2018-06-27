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
    /// ��������
    /// </summary>
    [TypeFilter(typeof(AppRuntimeFilter))]
    public class BaseController : Controller
    {
        /// <summary>
        /// ��ǰ��¼�û���Ϣ
        /// </summary>
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

        /// <summary>
        /// Ӧ�ó�������
        /// </summary>
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