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
    /// 登录注册控制器
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
                var apiUrl = _AppConfig.WebApiHost + "/api/AppUser/GetUserByUserName?userName=" + viewModel.UserName;
                var appUser = APIInvoker.Get<Models.Account.AppUserModel>(apiUrl);

                if (appUser != null) {
                    if (appUser.Enable == 0) {
                        // 用户已被禁用
                        var loginResult = new Models.Account.LoginResult(101, viewModel.UserName);
                        TempData["ErrorInfo"] = JsonConvert.SerializeObject(loginResult);
                        return RedirectToAction("Login", "Account");
                    }
                    else if(appUser.Password != Md5Encrypt.Md5Bit32(viewModel.Password)) {
                        // 密码有误
                        var loginResult = new Models.Account.LoginResult(102, viewModel.UserName);
                        TempData["ErrorInfo"] = JsonConvert.SerializeObject(loginResult);
                        return RedirectToAction("Login", "Account");
                    }
                    else {                        
                        // 生成一个token并写入cookie，该token以用户名+密码作为用户凭据
                        string tokenContent = appUser.UserName + "," + appUser.Password;
                        string token = DesEncrypt.Encrypt(tokenContent, _AppConfig.DesEncryptKey);
                        var cookieOptions = new CookieOptions(){
                            Expires = DateTime.Now.AddDays(_AppConfig.CookieExpire)
                        };
                        Response.Cookies.Append("token", token, cookieOptions);

                        // 将user信息DES加密后写入cookie
                        string userInfo = DesEncrypt.Encrypt(JsonConvert.SerializeObject(appUser), _AppConfig.DesEncryptKey);
                        Response.Cookies.Append("user", userInfo, cookieOptions);
                        // 将username写入cookie
                        Response.Cookies.Append("user_name", appUser.UserName, cookieOptions);

                        // 将token保存在服务器端缓存中(永不过期)
                        var cacheOptons = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(30));
                        _memoryCache.Set("token_" + appUser.Id, token, cacheOptons);
                        
                        // 加一层session：新的session方案，n小时不使用系统自动掉线
                        HttpContext.Session.SetString("token_" + appUser.Id, token);//将token写入session

                        return RedirectToAction("List", "Resource");
                    }
                }
                else {
                    // 用户名不存在
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

        [HttpPost]
        public IActionResult UpdatePasswordHandler(Models.Account.PasswordModel viewModel)
        {
            if (viewModel != null) {
                viewModel.Password = Md5Encrypt.Md5Bit32(viewModel.Password);
                string apiUrl = _AppConfig.WebApiHost + "/api/AppUser/UpdatePassword?userId=" + viewModel.UserId + "&password=" + viewModel.Password;
                bool result = APIInvoker.Get<bool>(apiUrl);
                if (result) {
                    // 生成新token
                    string tokenContent = _AppUser.UserName + "," + viewModel.Password;
                    string token = DesEncrypt.Encrypt(tokenContent, _AppConfig.DesEncryptKey);
                    // 更新缓存中的token：将token保存在服务器端缓存中(永不过期)
                    var cacheOptons = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(30));
                    _memoryCache.Set("token_" + viewModel.UserId, token, cacheOptons);
                    // 更新cookie中的token
                    var cookieOptions = new CookieOptions() {
                        Expires = DateTime.Now.AddDays(_AppConfig.CookieExpire)
                    };
                    Response.Cookies.Append("token", token, cookieOptions);

                    // 加一层session：新的session方案，n小时不使用系统自动掉线
                    HttpContext.Session.SetString("token_" + viewModel.UserId, token);//将token写入session
                }
                TempData["result"] = result;
            }
            return RedirectToAction("UpdatePassword");
        }

        public IActionResult UserInfo()
        {
            var apiUrl = _AppConfig.WebApiHost + "/api/AppUser/GetUserById?id=" + _AppUser.Id;
            Models.Account.UserInfoModel viewModel = APIInvoker.Get<Models.Account.UserInfoModel>(apiUrl);

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult UserInfoHandler(Models.Account.UserInfoModel viewModel)
        {
            if (viewModel != null) { 
                var userGetApi = _AppConfig.WebApiHost + "/api/AppUser/GetUserById?id=" + viewModel.Id;
                Models.Account.UserInfoModel appUser = APIInvoker.Get<Models.Account.UserInfoModel>(userGetApi);
                appUser.Mobile = viewModel.Mobile?.Trim();
                appUser.Email = viewModel.Email?.Trim();
                appUser.UpdateTime = DateTime.Now;

                var userUpdateApi = _AppConfig.WebApiHost + "/api/AppUser/UpdateUser";
                bool result = APIInvoker.Post<bool>(userUpdateApi, appUser);

                TempData["result"] = result;
            }
            return RedirectToAction("UserInfo");
        }

        [TypeFilter(typeof(AnonymousFilter))]
        public IActionResult FindMyPassword()
        {
            Models.Account.FindMyPasswordModel viewModel = new Models.Account.FindMyPasswordModel();
            return View(viewModel);
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="email"></param>
        /// <returns>-1=邮箱不存在，0=发送失败，1=发送成功</returns>
        [HttpGet]
        [TypeFilter(typeof(AnonymousFilter))]
        public int SendVerCode(string email)
        {
            // 首先判断Email是否存在
            var appUser = AppDTO.GetUserByEmail(email);
            if(appUser == null || appUser.Id <= 0) {
                return -1;
            }

            int result = 0;

            string code = new Random().Next(1000, 9999).ToString();
            string content = "您的验证码是：" + code + "，请在5分钟内使用。";
            bool res = new MailHelper().SendMail(email, content, "CRM系统验证码", "验证码", "验证码");
            if (res) {
                try { 
                    // 序列化成json，将加密后的json写入cookie
                    string jsonText = JsonConvert.SerializeObject(new { email = email, code = code });
                    string encryptedJsonText = DesEncrypt.Encrypt(jsonText, _AppConfig.DesEncryptKey);

                    // cookie有效期5分钟
                    var cookieOptions = new CookieOptions() {
                        Expires = DateTime.Now.AddMinutes(5)
                    };
                    Response.Cookies.Append("_pwdc", encryptedJsonText, cookieOptions);// cookie找回有关的验证码信息

                    result = 1;
                }
                catch {
                    result = 0;
                }
            }
            else {
                result = 0;
            }
            return result;
        }

        // 重置密码
        [HttpPost]
        [TypeFilter(typeof(AnonymousFilter))]
        public int ResetPassword()
        {
            string email = Request.Form["email"];
            string password = Request.Form["password"];
            string code = Request.Form["code"];
            
            // 首先判断验证码是否正确
            string encryptedCode = Request.Cookies["_pwdc"];
            string jsonText = DesEncrypt.Decrypt(encryptedCode, _AppConfig.DesEncryptKey);

            var t = new
            {
                email = "",
                code = ""
            };
            var json = JsonConvert.DeserializeAnonymousType(jsonText, t);
            if(json.code == code) {
                // 验证码正确，修改密码
                string md5_pasword = Md5Encrypt.Md5Bit32(password);
                var appUser = AppDTO.GetUserByEmail(email);
                if (appUser != null) { 
                    string apiUrl = _AppConfig.WebApiHost + "/api/AppUser/UpdatePassword?userId=" + appUser.Id + "&password=" + md5_pasword;
                    bool result = APIInvoker.Get<bool>(apiUrl);
                    if (result) {
                        return 1;//修改成功
                    }
                    else {
                        return 0;//修改失败
                    }
                }
                else {
                    return -2;//用户不存在
                }
            }
            else {
                return -1;//验证码无效
            }
        }
    }
}