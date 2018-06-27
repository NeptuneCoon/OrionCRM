using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.CRM.WebAPI.Controllers
{
    /// <summary>
    /// 测试用的Controller，可以删除
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class TestController : Controller
    {
        //private readonly IDistributedCache _distributedCache;
        //public TestController(IDistributedCache distributedCache)
        //{
        //    _distributedCache = distributedCache;
        //}

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

        [HttpGet]
        public string TestRedis()
        {
            /*
            // 写入<set>
            var data = System.Text.Encoding.UTF8.GetBytes("编译原理");
            _distributedCache.Set("book", data);

            // 获取<get>
            var cachedData = _distributedCache.Get("book");           
            var cachedMessage = System.Text.Encoding.UTF8.GetString(cachedData);


            // 写入<setString>
            _distributedCache.SetString("movie", "星际穿越");
            // 读取<getString>
            string str2 = _distributedCache.GetString("movie");

            // 写入<setString，带过期时间>
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
            _distributedCache.SetString("team", "火箭队", options);
            */

            //RedisClient redisClient = new RedisClient(_distributedCache);
            //redisClient.Set("name", "Harden");
            //redisClient.Set("name2", "CP3", DateTime.Now.AddMinutes(1));

            /*
            RedisCacheOptions options = new RedisCacheOptions();
            options.Configuration = GetConfigurationSettings("Redis:Connection");

            RedisClient redisClient = new RedisClient(options);
            bool r1 = redisClient.Set("name", "zhang");
            bool r2 = redisClient.Set("name2", "zhang2", TimeSpan.FromMinutes(1));
            */

            return "1";
        }

        [HttpGet]
        public APIDataResult BatchInsert()
        {
            try {
                APIDataResult dataResult = new APIDataResult();
                return dataResult;
            }
            catch (Exception ex) {
                APIDataResult dataResult = new APIDataResult(-1, null, ex.Message);
                return dataResult;
            }
        }
    }
}
