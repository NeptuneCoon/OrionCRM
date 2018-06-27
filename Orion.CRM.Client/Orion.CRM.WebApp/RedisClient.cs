using Microsoft.Extensions.Caching.Redis;
using Newtonsoft.Json;
//using StackExchange.Redis;
using System;
using System.Text;

namespace Orion.CRM.WebApp
{
    /*
    public class RedisClient
    {
        protected IDatabase _cache;
        private ConnectionMultiplexer _connection;

        public RedisClient(RedisCacheOptions options, int database = 0)
        {
            _connection = ConnectionMultiplexer.Connect(options.Configuration);
            _cache = _connection.GetDatabase(database);
        }

        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public bool Set(string key, object value)
        {
            if (key == null) {
                throw new ArgumentNullException(nameof(key));
            }
            return _cache.StringSet(key, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)));
        }


        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        public bool Set(string key, object value, TimeSpan expire)
        {
            if (key == null) {
                throw new ArgumentNullException(nameof(key));
            }
            //DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            //options.AbsoluteExpiration = expire;

            return _cache.StringSet(key, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)), expire);
        }


        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public T Get<T>(string key) where T : class
        {
            if (key == null) {
                throw new ArgumentNullException(nameof(key));
            }

            var value = _cache.StringGet(key);

            if (!value.HasValue) {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public object Get(string key)
        {
            if (key == null) {
                throw new ArgumentNullException(nameof(key));
            }

            var value = _cache.StringGet(key);

            if (!value.HasValue) {
                return null;
            }
            return value;
        }


        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            if (key == null) {
                throw new ArgumentNullException(nameof(key));
            }

            return _cache.KeyDelete(key);
        }
    }
    */
}
