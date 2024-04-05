using System.Runtime.Caching;

namespace Simple.Utils.Helper
{
    /// <summary>基于 System.Runtime.Caching 的内存缓存帮助类</summary>
    public class MemoryCacheHelper
    {
        #region # 写入缓存（无过期时间） —— void Set<T>(string key, T value)

        /// <summary>写入缓存（无过期时间）</summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void Set<T>(string key, T value)
        {
            LockHelper.LockRun(key, () =>
            {
                //如果缓存已存在则清空
                if (MemoryCache.Default.Get(key) != null)
                {
                    MemoryCache.Default.Remove(key);
                }

                CacheItemPolicy policy = new CacheItemPolicy
                {
                    Priority = CacheItemPriority.NotRemovable
                };

                MemoryCache.Default.Set(key, value, policy);
            });
        }

        #endregion # 写入缓存（无过期时间） —— void Set<T>(string key, T value)

        #region # 写入缓存（有过期时间） —— void Set<T>(string key, T value, DateTime exp)

        /// <summary>写入缓存（有过期时间）</summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="exp">过期时间</param>
        public static void Set<T>(string key, T value, DateTime exp)
        {
            LockHelper.LockRun(key, () =>
            {
                //如果缓存已存在则清空
                if (MemoryCache.Default.Get(key) != null)
                {
                    MemoryCache.Default.Remove(key);
                }

                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = exp;

                MemoryCache.Default.Set(key, value, policy);
            });
        }

        #endregion # 写入缓存（有过期时间） —— void Set<T>(string key, T value, DateTime exp)

        #region # 读取缓存 —— T Get<T>(string key)

        /// <summary>读取缓存</summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static T Get<T>(string key)
        {
            T instance = (T)MemoryCache.Default.Get(key);

            return instance;
        }

        /// <summary>获取或设置缓存 有过期时间，缓存不存在时，掉用接口设置缓存</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T GetOrSet<T>(string key, Func<T> func, DateTime? exp = null)
        {
            var t = Get<T>(key);
            if (t == null)
            {
                LockHelper.LockRun(key, () =>
                {
                    t = func();
                    if (exp != null)
                    {
                        Set(key, t, exp.Value);
                    }
                    else
                    {
                        Set(key, t);
                    }
                });
            }
            return t;
        }

        #endregion # 读取缓存 —— T Get<T>(string key)

        #region # 移除缓存 —— void Remove(string key)

        /// <summary>移除缓存</summary>
        /// <param name="key">键</param>
        public static void Remove(string key)
        {
            MemoryCache.Default.Remove(key);
        }

        #endregion # 移除缓存 —— void Remove(string key)

        #region # 移除缓存 —— void RemoveRange(IEnumerable<string> keys)

        /// <summary>移除缓存</summary>
        /// <param name="keys">缓存键集</param>
        public static void RemoveRange(IEnumerable<string> keys)
        {
            #region # 验证

            keys = keys?.Distinct().ToArray() ?? new string[0];
            if (!keys.Any())
            {
                return;
            }

            #endregion # 验证

            foreach (string key in keys)
            {
                MemoryCache.Default.Remove(key);
            }
        }

        #endregion # 移除缓存 —— void RemoveRange(IEnumerable<string> keys)

        #region # 是否存在缓存 —— bool Exists(string key)

        /// <summary>是否存在缓存</summary>
        /// <param name="key">键</param>
        /// <returns>是否存在</returns>
        public static bool Exists(string key)
        {
            return MemoryCache.Default.Contains(key);
        }

        #endregion # 是否存在缓存 —— bool Exists(string key)
    }
}