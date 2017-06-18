using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Orion.CRM.WebApp.App_Data
{
    /// <summary>
    /// todo...
    /// </summary>
    public class MemcacheAdapter
    {
        private AppConfig _appConfig;
        private IMemcachedClient _memcachedClient;


        //MemcachedClient client = null;
        public MemcacheAdapter(string host, int port)
        {
            /*
            LoggerFactory factory = new LoggerFactory();
            Logger<MemcachedClient> logger = new Logger<MemcachedClient>(factory);

            IOptions<MemcachedClientOptions> options = ;
            MemcachedClientConfiguration mcc = new MemcachedClientConfiguration(logger, options);
            mcc.AddServer(host, port);
            mcc.KeyTransformer = new DefaultKeyTransformer();
            mcc.NodeLocator = typeof(DefaultNodeLocator);
            mcc.Transcoder = new DefaultTranscoder();
            mcc.SocketPool.MinPoolSize = 10;
            mcc.SocketPool.MaxPoolSize = 100;
            mcc.SocketPool.ConnectionTimeout = new System.TimeSpan(0, 0, 10);
            mcc.SocketPool.DeadTimeout = new System.TimeSpan(0, 0, 30);

            client = new MemcachedClient(mcc);
            */
        }

        public MemcacheAdapter(IMemcachedClient memcachedClient, IOptions<AppConfig> optionsAccessor)
        {
            _memcachedClient = memcachedClient;
            _appConfig = optionsAccessor.Value;
        }

        public void Add(string key, object value)
        {
            _memcachedClient.Add(key, value, _appConfig.MemcachedExpire);
        }

        public void Add(string key, object value, int cacheSeconds)
        {
            _memcachedClient.Add(key, value, cacheSeconds);
        }

        public object Get(string key)
        {
            return _memcachedClient.Get(key);
        }

        public T Get<T>(string key)
        {
            return _memcachedClient.Get<T>(key);
        }

        public IDictionary<string, object> Get(IEnumerable<string> keys)
        {
            return _memcachedClient.Get(keys);
        }

        public void Remove(string key)
        {
            _memcachedClient.Remove(key);
        }
    }
}
