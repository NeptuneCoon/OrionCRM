using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Orion.CRM.Application;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Orion.CRM.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class MenuPageController : Controller
    { 
        private MenuPageAppService service = new MenuPageAppService();
        //private RedisClient redisClient;
        //private string redisEnable = "";

       
        /*
        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <param name="key">配置节点，多级请以:分隔</param>
        /// <returns></returns>
        static string GetConfigurationSettings(string key)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            IConfigurationRoot Configuration = builder.Build();
            string value = Configuration[key];
            return value;
        }
        */

        public MenuPageController()
        {
            /*
            RedisCacheOptions options = new RedisCacheOptions();
            options.Configuration = GetConfigurationSettings("Redis:Connection");

            redisClient = new RedisClient(options);
            redisEnable = GetConfigurationSettings("Redis:RedisEnable");
            */
        }

        #region 菜单相关操作
        [HttpGet]
        public APIDataResult GetMenu(int id)
        {
            try {
                Entity.SystemMenu menu = service.GetMenu(id);
                APIDataResult dataResult = new APIDataResult(200, menu);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult GetParentMenus()
        {
            try {
                IEnumerable<Entity.SystemMenu> menus = service.GetParentMenus();
                APIDataResult dataResult = new APIDataResult(200, menus);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult GetAllMenus()
        {
            try {
                IEnumerable<Entity.SystemMenu> menus = service.GetAllMenus();
                APIDataResult dataResult = new APIDataResult(200, menus);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult GetChildMenus(int menuId)
        {
            try {
                IEnumerable<Entity.SystemMenu> childMenus = service.GetChildMenus(menuId);
                APIDataResult dataResult = new APIDataResult(200, childMenus);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult GetAllLevel2Menus()
        {
            try {
                IEnumerable<Entity.SystemMenu> level2Menus = service.GetAllLevel2Menus();
                APIDataResult dataResult = new APIDataResult(200, level2Menus);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpPost]
        public APIDataResult CreateMenu([FromBody]Entity.SystemMenu menu)
        {
            try {
                int primaryId = service.CreateMenu(menu);
                APIDataResult dataResult = new APIDataResult(200, primaryId);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }


        [HttpPost]
        public APIDataResult UpdateMenu([FromBody]Entity.SystemMenu menu)
        {
            try {
                bool res = service.UpdateMenu(menu);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        [HttpGet]
        public APIDataResult DeleteMenu(int menuId)
        {
            try {
                bool res = service.DeleteMenu(menuId);
                APIDataResult dataResult = new APIDataResult(200, res);
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }

        #region 获取某一角色的所有左侧菜单
        /// <summary>
        /// 获取某一角色的所有左侧菜单
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns></returns>
        /*
        [HttpGet]
        public APIDataResult GetAllLeftMenusFromRedis(int roleId)
        {
            if (roleId <= 0) {
                return new APIDataResult(-1, null, "参数错误：roleId不能小于0");
            }

            try {
                IEnumerable<Entity.RoleMenuComplex> leftMenus = null;
                if (redisEnable == "1") {
                    // 首先判断Redis缓存中是否存在
                    var cacheLeftMenus = redisClient.Get<IEnumerable<Entity.RoleMenuComplex>>("left_all_menu_" + roleId);//角色下的的二级菜单
                    if (cacheLeftMenus != null) {
                        // 缓存中存在，直接返回
                        leftMenus = cacheLeftMenus.Where(x => x.Parent != null).OrderBy(x => x.SortNo).AsEnumerable();//当前父菜单下的子菜单
                    }
                    else {
                        // 缓存中不存在
                        IEnumerable<Entity.RoleMenuComplex> roleMenus = new RoleAppService().GetComplexRoleMenusByRoleId(roleId);//从数据库读取

                        // 获得当前角色下的所有Left菜单
                        leftMenus = roleMenus.Where(x => x.Parent != null).OrderBy(x => x.SortNo);

                        // 写入缓存
                        redisClient.Set("left_all_menu_" + roleId, leftMenus);
                    }
                }
                else {
                    // 不使用缓存
                    IEnumerable<Entity.RoleMenuComplex> roleMenus = new RoleAppService().GetComplexRoleMenusByRoleId(roleId);//从数据库读取
                    leftMenus = roleMenus.Where(x => x.Parent != null).OrderBy(x => x.SortNo);
                }

                APIDataResult dataResult = new APIDataResult(200, leftMenus);
                return dataResult;
            }
            catch(Exception ex) {
                //Logger.Write("MenuPageController.cs中的GetAllLeftMenusFromRedis()方法发生异常：" + ex.Message);
                return new APIDataResult(-1, null, ex.Message);
            }
        }
        */
        [HttpGet]
        public APIDataResult GetAllLeftMenusFromRedis(int roleId)
        {
            if (roleId <= 0) {
                return new APIDataResult(-1, null, "参数错误：roleId不能小于0");
            }
            IEnumerable<Entity.RoleMenuComplex> roleMenus = new RoleAppService().GetComplexRoleMenusByRoleId(roleId);//从数据库读取(角色下所有菜单)
            var leftMenus = roleMenus?.Where(x => x.Parent != null)?.OrderBy(x => x.SortNo);
            // 写入日志
            //if (leftMenus == null || leftMenus.ToList().Count <= 0) {
            //    Logger.Write("MenuPageController.cs，方法名：GetAllLeftMenusFromRedis，返回结果leftMenus=null");
            //}

            APIDataResult dataResult = new APIDataResult(200, leftMenus);
            return dataResult;
        }
        #endregion


        #region 获取某一角色的指定顶部菜单下的左侧菜单
        /// <summary>
        /// 获取某一角色的指定顶部菜单下的左侧菜单
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="topId">顶部一级菜单Id</param>
        /// <returns></returns>
        /*
        [HttpGet]
        public APIDataResult GetLeftMenusFromRedis(int roleId, int topId)
        {
            if (roleId <= 0 || topId <= 0) {
                return new APIDataResult(-1, null, "参数错误：roleId或topId不能小于0");
            }

            try {
                IEnumerable<Entity.RoleMenuComplex> leftMenus = null;
                if (redisEnable == "1") {
                    //Logger.Write($"进入MenuPageController.GetLeftMenusFromRedis({roleId},{topId}).");
                    // 首先判断Redis缓存中是否存在
                    var cacheLeftMenus = redisClient.Get<IEnumerable<Entity.RoleMenuComplex>>("left_menu_" + roleId);//角色下的的二级菜单
                    if (cacheLeftMenus != null) {
                        //Logger.Write($"MenuPageController.GetLeftMenusFromRedis({roleId},{topId})，缓存cacheLeftMenus存在。");
                        // 缓存中存在，直接返回<注意缓存中存储的是当前角色的所有菜单，这里要根据topId做筛选后返回>
                        leftMenus = cacheLeftMenus.Where(x => x.Parent == topId).OrderBy(x => x.SortNo).AsEnumerable();//当前父菜单下的子菜单
                    }
                    else {
                        //Logger.Write($"MenuPageController.GetLeftMenusFromRedis({roleId},{topId})，缓存cacheLeftMenus不存在。");
                        // 缓存中不存在
                        IEnumerable<Entity.RoleMenuComplex> roleMenus = new RoleAppService().GetComplexRoleMenusByRoleId(roleId);//从数据库读取

                        //Logger.Write($"MenuPageController.GetLeftMenusFromRedis({roleId},{topId})，缓存cacheLeftMenus不存在。从数据库取到：");
                        //Logger.Write(Newtonsoft.Json.JsonConvert.SerializeObject(roleMenus));
                        // 写入缓存
                        redisClient.Set("left_menu_" + roleId, roleMenus.Where(x => x.Parent != null));

                        // 获得当前角色下的Left菜单<注意缓存中存储的是当前角色的所有菜单，这里要根据topId做筛选后返回>
                        leftMenus = roleMenus.Where(x => x.Parent == topId).OrderBy(x => x.SortNo);
                    }
                }
                else {
                    // 不使用缓存
                    leftMenus = new RoleAppService().GetComplexRoleMenusByRoleId(roleId);//从数据库读取
                }

                APIDataResult dataResult = new APIDataResult(200, leftMenus);
                //Logger.Write($"MenuPageController.GetLeftMenusFromRedis({roleId},{topId})最终返回结果：" + Newtonsoft.Json.JsonConvert.SerializeObject(dataResult));
                return dataResult;
            }
            catch(Exception ex) {
                //Logger.Write($"MenuPageController.cs中的GetLeftMenusFromRedis({roleId},{topId})方法发生异常：" + ex.Message);
                return new APIDataResult(-1, null, ex.Message);
            }
        }
        */
        [HttpGet]
        public APIDataResult GetLeftMenusFromRedis(int roleId, int topId)
        {
            if (roleId <= 0 || topId <= 0) {
                return new APIDataResult(-1, null, "参数错误：roleId或topId不能小于0");
            }
            var roleMenus = new RoleAppService().GetComplexRoleMenusByRoleId(roleId);//从数据库读取(角色下所有菜单)
            var leftMenus = roleMenus?.Where(x => x.Parent == topId)?.OrderBy(x => x.SortNo);
            // 写入日志
            //if (leftMenus == null || leftMenus.ToList().Count <= 0) {
            //    Logger.Write("MenuPageController.cs，方法名：GetLeftMenusFromRedis，返回结果leftMenus=null");
            //}

            APIDataResult dataResult = new APIDataResult(200, leftMenus);
            return dataResult;
        }
        #endregion

        #region 获取顶部菜单
        /// <summary>
        /// 获取顶部菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        /*
        [HttpGet]
        public APIDataResult GetTopMenusFromRedis(int roleId)
        {
            if (roleId <= 0) {
                return new APIDataResult(-1, null, "参数错误：roleId不能小于0");
            }

            try {
                IEnumerable<Entity.RoleMenuComplex> topMenus = null;
                if (redisEnable == "1") {
                    // 首先判断Redis缓存中是否存在
                    var cacheTopMenus = redisClient.Get<IEnumerable<Entity.RoleMenuComplex>>("top_menu_" + roleId);//角色下的一级菜单
                    if (cacheTopMenus != null) {
                        // 缓存中存在，直接返回
                        topMenus = cacheTopMenus.OrderBy(x => x.SortNo).AsEnumerable();
                    }
                    else {
                        // 缓存中不存在
                        IEnumerable<Entity.RoleMenuComplex> roleMenus = new RoleAppService().GetComplexRoleMenusByRoleId(roleId);//从数据库读取

                        // 获得当前角色下的Top菜单
                        topMenus = roleMenus.Where(x => x.Parent == null).OrderBy(x => x.SortNo);

                        // 写入缓存
                        redisClient.Set("top_menu_" + roleId, topMenus);
                    }
                }
                else {
                    // 不使用缓存
                    IEnumerable<Entity.RoleMenuComplex> roleMenus = new RoleAppService().GetComplexRoleMenusByRoleId(roleId);//从数据库读取
                    topMenus = roleMenus.Where(x => x.Parent == null).OrderBy(x => x.SortNo);
                }


                APIDataResult dataResult = new APIDataResult(200, topMenus);
                return dataResult;
            }
            catch(Exception ex) {
                //Logger.Write($"MenuPageController.cs中的GetTopMenusFromRedis({roleId})方法发生异常：" + ex.Message);
                return new APIDataResult(-1, null, ex.Message);
            }
        }
        */
        [HttpGet]
        public APIDataResult GetTopMenusFromRedis(int roleId)
        {
            if (roleId <= 0) {
                return new APIDataResult(-1, null, "参数错误：roleId不能小于0");
            }
            IEnumerable<Entity.RoleMenuComplex> roleMenus = new RoleAppService().GetComplexRoleMenusByRoleId(roleId);//从数据库读取(角色下所有菜单)
            var topMenus = roleMenus?.Where(x => x.Parent == null)?.OrderBy(x => x.SortNo);
            // 写入日志
            //if (topMenus == null || topMenus.ToList().Count <= 0) {
            //    Logger.Write("MenuPageController.cs，方法名：GetTopMenusFromRedis，返回结果topMenus=null");
            //}

            APIDataResult dataResult = new APIDataResult(200, topMenus);
            return dataResult;
        }
        #endregion


        #endregion
    }
}
