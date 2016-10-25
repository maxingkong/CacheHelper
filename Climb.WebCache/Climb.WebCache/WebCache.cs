using System;
using System.Web.Caching;
using Climb.ICacheProvider;
using System.Web;

namespace Climb.WebCache
{
    /// <summary>
    /// web cache 封装
    /// </summary>
    public class WebCache : ICacheProvde
    {
        protected static Cache WebDataCache = HttpRuntime.Cache;

        private long _defuaultTime = 1200;//默认缓存20分钟

        #region 构造函数
        public WebCache()
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="time"></param>
        public WebCache(int time)
        {
            TimeOut = time;
        }
        #endregion

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存键值</param>
        /// <param name="obj">缓存对象</param>
        public void AddObject(string key, object obj)
        {
            WebDataCache.Insert(key, obj);
        }

        public void AddObject(string key, object obj, TimeExpirationEnum timeEnum)
        {

            if (timeEnum == TimeExpirationEnum.Absolute)
            {
                WebDataCache.Insert(key, obj, null, DateTime.Now.AddSeconds(_defuaultTime), Cache.NoSlidingExpiration);
            }
            else
            {
                WebDataCache.Insert(key, obj, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(_defuaultTime));
            }

        }


        public void AddObject(string key, object obj, TimeExpirationEnum timeEnum, Action<string> doAction)
        {
            CacheItemRemovedCallback removedCallback =
                delegate(string keys, object cacheItem, CacheItemRemovedReason removedReason)
                {
                    if (removedReason == CacheItemRemovedReason.Expired)
                    {
                        doAction(key);
                    }
                };

            if (timeEnum == TimeExpirationEnum.Absolute)
            {
                WebDataCache.Insert(key, obj, null, DateTime.Now.AddSeconds(_defuaultTime), Cache.NoSlidingExpiration, CacheItemPriority.Normal, removedCallback);
            }
            else
            {
                WebDataCache.Insert(key, obj, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(_defuaultTime), CacheItemPriority.Normal, removedCallback);
            }
        }

        public void SetObject(string key, object obj)
        {
            WebDataCache[key] = obj;
        }

        public void RemoveObject(string key)
        {
            WebDataCache.Remove(key);
        }

        public T GetObject<T>(string key) where T : class
        {
            return (T)WebDataCache.Get(key);
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

        public long TimeOut
        {
            get { return _defuaultTime; }
            set { _defuaultTime = value; }
        }

        public override string ToString()
        {
            return "WebCache";
        }
    }
}
