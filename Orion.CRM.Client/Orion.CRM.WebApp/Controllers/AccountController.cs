using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orion.CRM.WebTools;
using Orion.CRM.WebApp.App_Data;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace Orion.CRM.WebApp.Controllers
{
    /// <summary>
    /// ��¼ע�������
    /// </summary>
    public class AccountController : BaseController
    {
        private IMemoryCache _memoryCache;
        public AccountController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [TypeFilter(typeof(AnonymousFilter))]
        public IActionResult Login()
        {
            Models.Account.LoginViewModel viewModel = new Models.Account.LoginViewModel();

            if (TempData["ErrorInfo"] != null) { 
                Models.Account.LoginResult loginResult = JsonConvert.DeserializeObject<Models.Account.LoginResult>(TempData["ErrorInfo"].ToString());
                if(loginResult != null) {
                    ViewBag.ErrorCode = loginResult.ErrorCode;
                    viewModel.UserName = loginResult.UserName;
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [TypeFilter(typeof(AnonymousFilter))]
        public IActionResult LoginHandler(Models.Account.LoginViewModel viewModel)
        {
            if (viewModel != null) {
                var apiUrl = _AppConfig.WebApiHost + "api/AppUser/GetUserByUserName?userName=" + viewModel.UserName;
                var appUser = APIInvoker.Get<Models.Account.AppUserModel>(apiUrl);

                if (appUser != null) {
                    if (appUser.Enable == 0) {
                        // �û��ѱ�����
                        var loginResult = new Models.Account.LoginResult(101, viewModel.UserName);
                        TempData["ErrorInfo"] = JsonConvert.SerializeObject(loginResult);
                        return RedirectToAction("Login", "Account");
                    }
                    else if(appUser.Password != Md5Encrypt.Md5Bit32(viewModel.Password)) {
                        // ��������
                        var loginResult = new Models.Account.LoginResult(102, viewModel.UserName);
                        TempData["ErrorInfo"] = JsonConvert.SerializeObject(loginResult);
                        return RedirectToAction("Login", "Account");
                    }
                    else {
                        // ����һ��token��д��cookie����token���û���+������Ϊ�û�ƾ��
                        string tokenContent = appUser.UserName + "," + appUser.Password;
                        string token = DesEncrypt.Encrypt(tokenContent, _AppConfig.DesEncryptKey);
                        var cookieOptions = new CookieOptions(){
                            Expires = DateTime.Now.AddDays(_AppConfig.CookieExpire)
                        };
                        Response.Cookies.Append("token", token, cookieOptions);

                        // ��user��ϢDES���ܺ�д��cookie
                        string userInfo = DesEncrypt.Encrypt(JsonConvert.SerializeObject(appUser), _AppConfig.DesEncryptKey);
                        Response.Cookies.Append("user", userInfo, cookieOptions);
                        // ��usernameд��cookie
                        Response.Cookies.Append("user_name", appUser.UserName, cookieOptions);

                        // ��token�����ڷ������˻�����(��������)
                        var cacheOptons = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(30));
                        _memoryCache.Set("token_" + appUser.Id, token, cacheOptons);
                        // ���û���Ϣ�����ڷ������˻�����(��������)
                        //_memoryCache.Set("user_" + appUser.Id, appUser, cacheOptons);

                        return RedirectToAction("List", "Resource");
                    }
                }
                else {
                    // �û���������
                    var loginResult = new Models.Account.LoginResult(103, viewModel.UserName);
                    TempData["ErrorInfo"] = JsonConvert.SerializeObject(loginResult);
                    return RedirectToAction("Login", "Account");
                }
            }

            return RedirectToAction("List", "Resource");
        }

        public IActionResult Logout()
        {
            _memoryCache.Remove("token_" + _AppUser.Id);
            //_memoryCache.Remove("user_" + _AppUser.Id);

            Response.Cookies.Delete("token");
            Response.Cookies.Delete("user");
            Response.Cookies.Delete("user_name");

            return RedirectToAction("Login");
        }

        public IActionResult UpdatePassword()
        {
            Models.Account.PasswordModel viewModel = new Models.Account.PasswordModel();
            viewModel.UserId = _AppUser.Id;
            return View(viewModel);
        }

        public IActionResult UserInfo()
        {
            var apiUrl = _AppConfig.WebApiHost + "api/AppUser/GetUserById?id=" + _AppUser.Id;
            Models.Account.UserInfoModel viewModel = APIInvoker.Get<Models.Account.UserInfoModel>(apiUrl);

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult UserInfoHandler(Models.Account.UserInfoModel viewModel)
        {
            if (viewModel != null) { 
                var userGetApi = _AppConfig.WebApiHost + "api/AppUser/GetUserById?id=" + viewModel.Id;
                Models.Account.UserInfoModel appUser = APIInvoker.Get<Models.Account.UserInfoModel>(userGetApi);
                appUser.Mobile = viewModel.Mobile.Trim();
                appUser.Email = viewModel.Email.Trim();
                appUser.UpdateTime = DateTime.Now;

                var userUpdateApi = _AppConfig.WebApiHost + "api/AppUser/UpdateUser";
                bool result = APIInvoker.Post<bool>(userUpdateApi, appUser);

                TempData["result"] = result;
            }
            return RedirectToAction("UserInfo");
        }


        [HttpPost]
        public IActionResult UpdatePasswordHandler(Models.Account.PasswordModel viewModel)
        {
            if (viewModel != null) {
                viewModel.Password = Md5Encrypt.Md5Bit32(viewModel.Password);
                string apiUrl = _AppConfig.WebApiHost + "api/AppUser/UpdatePassword?userId=" + viewModel.UserId + "&password=" + viewModel.Password;
                bool result = APIInvoker.Get<bool>(apiUrl);
                if (result) {
                    // ����token
                    string tokenContent = _AppUser.UserName + "," + viewModel.Password;
                    string token = DesEncrypt.Encrypt(tokenContent, _AppConfig.DesEncryptKey);
                    // ��token�����ڷ������˻�����(��������)
                    var cacheOptons = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(30));
                    _memoryCache.Set("token_" + viewModel.UserId, token, cacheOptons);
                }
                TempData["result"] = result;
            }
            return RedirectToAction("UpdatePassword");
        }
    }
}