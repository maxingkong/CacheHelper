using System;
using Climb.ICacheProvider;
using System.Runtime.Caching;

namespace Climb.NetCache
{
    /// <summary>
    /// netcache 缓存
    /// </summary>
    public class NetCache : ICacheProvde
    {
        /// <summary>
        /// 默认缓存时间
        /// </summary>
        private long _defuaultTime = 3600;
        private static readonly ObjectCache MCache = MemoryCache.Default;

        public NetCache()
        {

        }

        /// <summary>
        /// 构造函数 初始化缓存时间
        /// </summary>
        /// <param name="time"></param>
        public NetCache(long time)
        {
            _defuaultTime = time;
        }

        public void AddObject(string key, object obj)
        {
            MCache.Add(key, obj, null);
        }

        public void AddObject(string key, object obj, TimeExpirationEnum timeEnum)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            if (timeEnum == TimeExpirationEnum.Absolute)
            {
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(TimeOut);
            }
            else
            {
                TimeSpan ts = TimeSpan.FromSeconds(TimeOut);
                policy.SlidingExpiration = ts;
            }
            MCache.Add(key, obj, policy);
        }

        /// <summary>
        /// 添加一个过期回调的 方法
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="timeEnum"></param>
        /// <param name="doAction"></param>
        public void AddObject(string key, object obj, TimeExpirationEnum timeEnum, Action<string> doAction)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            if (timeEnum == TimeExpirationEnum.Absolute)
            {
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(TimeOut);
            }
            else
            {
                TimeSpan ts = TimeSpan.FromSeconds(TimeOut);
                policy.SlidingExpiration = ts;
            }
            CacheEntryRemovedCallback removedCallback = delegate(CacheEntryRemovedArguments cera)
            {
                if (cera.RemovedReason == CacheEntryRemovedReason.Expired)
                {
                    doAction(key);
                }

            };
            policy.RemovedCallback = removedCallback;
            MCache.Add(key, obj, policy);
        }


        public void SetObject(string key, object obj)
        {
            MCache[key] = obj;
        }

        public void RemoveObject(string key)
        {
            MCache.Remove(key);
        }

        public T GetObject<T>(string key) where T : class
        {
            return (T)MCache.Get(key);
        }

        public T GetObject<T>(string key, Func<T> tdataFunc) where T : class
        {
            T tObj = GetObject<T>(key);
            if (tObj != null) return tObj;
            tObj = tdataFunc();
            AddObject(key, tObj);
            return tObj;
        }

        public T GetObject<T>(string key, Func<T> tdataFunc, TimeExpirationEnum timeoffEnum) where T : class
        {
            T tObj = GetObject<T>(key);
            if (tObj != null) return tObj;
            tObj = tdataFunc();
            AddObject(key, tObj, timeoffEnum);
            return tObj;
        }

        /// <summary>
        /// 缓存过期时间 
        /// </summary>
        public long TimeOut
        {
            get { return _defuaultTime; }
            set { _defuaultTime = value; }
        }

        /// <summary>
        /// 重载
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "NetCache";
        }
    }
}
