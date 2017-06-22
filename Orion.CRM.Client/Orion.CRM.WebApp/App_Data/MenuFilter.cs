using Enyim.Caching;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Orion.CRM.WebApp.Models.Account;
using Orion.CRM.WebTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebApp.App_Data
{
    /// <summary>
    /// 菜单过滤器
    /// 该过滤器会将用户的菜单操作相关信息写入cookie
    /// 菜单权限控制同样在此过滤器中进行
    /// </summary>
    public class MenuFilter : IActionFilter
    {
        private readonly AppConfig _appConfig;
        private IMemcachedClient _memcachedClient;
        public MenuFilter(IOptions<AppConfig> optionsAccessor, IMemcachedClient memcachedClient)
        {
            _appConfig = optionsAccessor.Value;
            _memcachedClient = memcachedClient;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var isAllowAnonymous = context.Filters.FirstOrDefault(x => x.ToString().IndexOf("AnonymousFilter") >= 0);
            if (isAllowAnonymous != null) {
                return;
            }

            // 获取本次操作的/Controller/Action
            string displayName = context.ActionDescriptor.DisplayName;
            int p = displayName.IndexOf(" ");
            if (p <= 0) p = displayName.Length;
            displayName = displayName.Substring(0, p);

            // 如果本次操作的/Controller/Action在LeftMenu中，则说明操作的是菜单，需要写入Cookie，以便做菜单的状态保持
            string[] nameArr = displayName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (nameArr != null && nameArr.Length > 0) {
                string action = nameArr[nameArr.Length - 1];
                string controller = nameArr[nameArr.Length - 2].Replace("Controller", "");

                string menuUrl = "/" + controller + "/" + action;

                // 从cookie中取出appUser对象
                string encryptUserInfo = context.HttpContext.Request.Cookies["user"];// cookie中加密的用户信息
                if (string.IsNullOrEmpty(encryptUserInfo)) {
                    context.HttpContext.Response.Redirect("http://localhost:12816/Account/Login");
                }
                else {
                    AppUserModel appUser = JsonConvert.DeserializeObject<AppUserModel>(DesEncrypt.Decrypt(encryptUserInfo, _appConfig.DesEncryptKey));
                    // 取出角色下的菜单
                    if (appUser != null) {
                        var leftMenus = GetLeftMenus(appUser.RoleId);
                        if (leftMenus != null && leftMenus.Result != null) {
                            var clickMenu = leftMenus.Result.FirstOrDefault(x => x.URL == menuUrl);
                            if (clickMenu != null) {
                                int leftMenuId = clickMenu.MenuId;// 本次点击的二级菜单
                                int topMenuId = Convert.ToInt32(clickMenu.Parent);// 本次点击的父级菜单

                                // 上次操作的父级菜单
                                int lastTopMenuId = Convert.ToInt32(context.HttpContext.Session.GetInt32("TopMenuId"));

                                // 写入Session
                                if (lastTopMenuId == topMenuId) {
                                    // 将本次操作的二级菜单Id写入Session
                                    context.HttpContext.Session.SetInt32("LeftMenuId", leftMenuId);
                                }
                                else {
                                    // 将本次操作的新的一级菜单Id(topMenuId)写入Session
                                    context.HttpContext.Session.SetInt32("TopMenuId", topMenuId);

                                    // 如果是新点击的菜单，则二级菜单的id为默认为第一个
                                    var firstLeftMenu = leftMenus.Result.Where(x => x.Parent == topMenuId).OrderBy(x => x.SortNo).FirstOrDefault();
                                    if (firstLeftMenu != null) {
                                        leftMenuId = firstLeftMenu.MenuId;

                                        // 将本次操作的二级菜单Id(leftMenuId)写入Session
                                        context.HttpContext.Session.SetInt32("LeftMenuId", leftMenuId);
                                    }
                                }
                            }
                            else {
                                // 有可能操作的是非菜单Controller/Action，无须处理
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 判断当前操作的Controller/Action是否是一个菜单
        /// 如果是菜单，则需要做权限控制
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private bool IsMenu(string controller, string action)
        {
            // todo...
            bool res = false;


            return res;
        }

        private Task<IEnumerable<Models.Role.RoleMenuComplex>> GetLeftMenus(int roleId)
        {
            // 首先判断Memcached缓存中是否存在
            var cacheLeftMenus = _memcachedClient.Get<IEnumerable<Models.Role.RoleMenuComplex>>("left_menu_" + roleId);//角色下的的二级菜单
            if (cacheLeftMenus != null) {
                // 缓存中存在，直接返回
                var result = Task.FromResult(cacheLeftMenus);
                return result;
            }
            else {
                // 缓存中不存在，从数据库中获取，并写入缓存
                string apiUrl = _appConfig.WebApiHost + "api/Role/GetComplexRoleMenusByRoleId?roleId=" + roleId;
                var roleMenus = APIInvoker.Get<List<Models.Role.RoleMenuComplex>>(apiUrl);//角色下的所有菜单
                var leftMenus = roleMenus.Where(x => x.Parent != null);//角色下的二级菜单
                // 写入缓存
                _memcachedClient.Add("left_menu_" + roleId, leftMenus, int.MaxValue);

                var result = Task.FromResult(leftMenus);
                return result;
            }
        }
    }
}
