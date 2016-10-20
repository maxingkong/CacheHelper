using System;

namespace Climb.ICacheProvider
{
    /// <summary>
    /// 过期时间枚举
    /// </summary>
    public enum TimeExpirationEnum
    {
        /// <summary>
        /// 绝对过期
        /// </summary>
        Absolute,//绝对过期
        /// <summary>
        /// //滑动过期
        /// </summary>
        Slipping//滑动过期
    }


    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface ICacheProvde
    {
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存键值</param>
        /// <param name="obj">缓存对象</param>
        void AddObject(string key, object obj);

        /// <summary>
        /// 添加指定ID的对象
        /// </summary>
        /// <param name="key">缓存键值</param>
        /// <param name="obj">缓存对象</param>
        /// <param name="timeEnum">过期时间枚举</param>
        void AddObject(string key, object obj, TimeExpirationEnum timeEnum);


        /// <summary>
        /// 添加指定ID的对象 增加一个过期回调的方法
        /// </summary>
        /// <param name="key">缓存键值</param>
        /// <param name="obj">缓存对象</param>
        /// <param name="timeEnum">过期时间枚举</param>
        /// <param name="doAction">过期时间所触发的回调函数</param>
        void AddObject(string key, object obj, TimeExpirationEnum timeEnum, Action<string> doAction);

        /// <summary>
        /// 修改cache
        /// </summary>
        /// <param name="key">缓存键值</param>
        /// <param name="obj">缓存对象</param>
        void SetObject(string key, object obj);

        /// <summary>
        /// 移除指定ID的对象
        /// </summary>
        /// <param name="key">缓存键值</param>
        void RemoveObject(string key);

        /// <summary>
        /// 返回指定ID的对象
        /// </summary>
        /// <param name="key">缓存键值</param>
        /// <returns>返回缓存的对象</returns>
        T GetObject<T>(string key) where T : class;

        /// <summary>
        /// 返回指定ID的对象
        /// </summary>
        /// <param name="key">缓存键值</param>
        /// <param name="tdataFunc">没有则触发一个回调函数</param>
        /// <returns>返回缓存的对象</returns>
        T GetObject<T>(string key, Func<T> tdataFunc) where T : class;


        /// <summary>
        /// 返回指定ID的对象
        /// </summary>
        /// <param name="key">缓存键值</param>
        /// <param name="tdataFunc">没有则触发一个回调函数</param>
        /// <param name="timeoffEnum">过期时间枚举</param>
        /// <returns>返回缓存的对象</returns>
        T GetObject<T>(string key, Func<T> tdataFunc, TimeExpirationEnum timeoffEnum) where T : class;

        /// <summary>
        /// 到期时间
        /// </summary>
        long TimeOut { set; get; }
    }
}
