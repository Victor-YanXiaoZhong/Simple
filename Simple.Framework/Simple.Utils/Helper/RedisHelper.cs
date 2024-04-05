using Newtonsoft.Json;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;

namespace Simple.Utils.Helper
{
    public class RedisHelper
    {
        private static readonly List<string> RedisHosts;
        private static readonly string RedisPwd = "";

        private static readonly int RedisDbIndex = 0;
        private static readonly object LockObject = new object();

        private static ConnectionMultiplexer _connection;

        public static RedLockFactory redLockFactory;

        static RedisHelper()
        {
            RedisHosts = ConfigHelper.GetValue("Redis:Hosts").Split(',').ToList();
            RedisPwd = ConfigHelper.GetValue("Redis:Pwd");
            try
            {
                RedisDbIndex = ConfigHelper.GetValue<int>("Redis:Database");
            }
            catch (Exception) { }

            if (RedisHosts.Count == 0)
            {
                throw new CustomException("Redis配置 Redis:Hosts 不存在，不能使用redis", "Redis配置 Redis:Hosts 不存在，不能使用redis");
            }
        }

        public static ConnectionMultiplexer Connection
        {
            get
            {
                if (_connection == null)
                {
                    lock (LockObject)
                    {
                        if (_connection == null || !_connection.IsConnected)
                        {
                            var config = new ConfigurationOptions
                            {
                                AbortOnConnectFail = false,
                                ConnectRetry = 10,
                                ConnectTimeout = 5000,
                                SyncTimeout = 5000,
                                AllowAdmin = true,
                                KeepAlive = 180,
                                Password = RedisPwd
                            };
                            RedisHosts.ForEach(host => config.EndPoints.Add(host));

                            _connection = ConnectionMultiplexer.Connect(config);
                            //注册如下事件
                            _connection.ConnectionFailed += MuxerConnectionFailed;
                            _connection.ErrorMessage += MuxerErrorMessage;
                            _connection.InternalError += MuxerInternalError;
                        }
                    }
                }
                return _connection;
            }
        }

        /// <summary>Redis 实现的分布式锁</summary>
        public static RedLockFactory RedlockFactory
        {
            get
            {
                if (redLockFactory != null) return redLockFactory;
                var multiplexers = new List<RedLockMultiplexer>()
                {
                    Connection
                };
                var redlockFactory = RedLockFactory.Create(multiplexers);
                return redlockFactory;
            }
        }

        /// <summary>获取redis的DB</summary>
        /// <returns></returns>
        private static IDatabase GetDatabase()
        {
            if (Connection == null || !Connection.IsConnected)
            {
                throw new Exception("redis连接有误");
            }
            return Connection.GetDatabase(RedisDbIndex);
        }

        #region 记录redis的连接日志

        /// <summary>连接失败 ， 如果重新连接成功你将不会收到这个通知</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
        }

        /// <summary>redis类库错误</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerInternalError(object sender, InternalErrorEventArgs e)
        {
        }

        /// <summary>发生错误时</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerErrorMessage(object sender, RedisErrorEventArgs e)
        {
        }

        #endregion 记录redis的连接日志

        #region 锁

        /// <summary>使用redis关键字 key 锁执行方法函数，默认超时120秒</summary>
        /// <param name="key">关键字 key00</param>
        /// <param name="func">执行方法函数</param>
        /// <param name="expSecond">默认超时120秒</param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public static bool LockRun(string key, Func<bool> func, int expSecond = 120)
        {
            var db = GetDatabase();
            try
            {
                var lockValue = db.LockTake(key, Environment.MachineName, TimeSpan.FromSeconds(expSecond));

                if (!lockValue) throw new CustomException($"key: {key}没有获取到锁，可能正在执行");

                return func();
            }
            catch (Exception ex)
            {
                throw new CustomException("Redis 锁执行过程函数没有成功", ex);
            }
            finally
            {
                db.LockRelease(key, Environment.MachineName);
            }
        }

        #endregion 锁

        #region String

        #region 同步方法

        /// <summary>保存单个key value</summary>
        /// <param name="key">Redis Key</param>
        /// <param name="value">保存的值</param>
        /// <param name="secondTimeout">过期时间</param>
        /// <returns></returns>
        public static bool StringSet(string key, string value, int secondTimeout = 0)
        {
            return Do(db => db.StringSet(key, value, secondTimeout == 0 ? null : TimeSpan.FromSeconds(secondTimeout)));
        }

        /// <summary>保存多个key value</summary>
        /// <param name="keyValues">键值对</param>
        /// <returns></returns>
        public static bool StringSet(List<KeyValuePair<RedisKey, RedisValue>> keyValues)
        {
            List<KeyValuePair<RedisKey, RedisValue>> newkeyValues =
                keyValues.Select(p => new KeyValuePair<RedisKey, RedisValue>(p.Key, p.Value)).ToList();
            return Do(db => db.StringSet(newkeyValues.ToArray()));
        }

        /// <summary>保存一个对象</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="secondTimeout"></param>
        /// <returns></returns>
        public static bool StringSet<T>(string key, T obj, int secondTimeout = 0)
        {
            var json = ConvertJson(obj);
            return Do(db => db.StringSet(key, json, secondTimeout == 0 ? null : TimeSpan.FromSeconds(secondTimeout)));
        }

        /// <summary>获取单个key的值</summary>
        /// <param name="key">Redis Key</param>
        /// <returns></returns>
        public static string StringGet(string key)
        {
            return Do(db => db.StringGet(key));
        }

        /// <summary>获取一个key的对象</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T StringGet<T>(string key)
        {
            return Do(db => ConvertObj<T>(db.StringGet(key)));
        }

        /// <summary>为数字增长val</summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public static long StringIncrement(string key, long val = 1)
        {
            return Do(db => db.StringIncrement(key, val));
        }

        /// <summary>为数字减少val</summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public static long StringDecrement(string key, long val = 1)
        {
            return Do(db => db.StringDecrement(key, val));
        }

        /// <summary>为数字增长val</summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public static double StringIncrement(string key, double val = 1)
        {
            return Do(db => db.StringIncrement(key, val));
        }

        /// <summary>为数字减少val</summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public static double StringDecrement(string key, double val = 1)
        {
            return Do(db => db.StringDecrement(key, val));
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>保存单个key value</summary>
        /// <param name="key">Redis Key</param>
        /// <param name="value">保存的值</param>
        /// <param name="secondTimeout">过期时间</param>
        /// <returns></returns>
        public static async Task<bool> StringSetAsync(string key, string value, int secondTimeout)
        {
            return await Do(db => db.StringSetAsync(key, value, TimeSpan.FromSeconds(secondTimeout)));
        }

        /// <summary>保存多个key value</summary>
        /// <param name="keyValues">键值对</param>
        /// <returns></returns>
        public static async Task<bool> StringSetAsync(List<KeyValuePair<RedisKey, RedisValue>> keyValues)
        {
            List<KeyValuePair<RedisKey, RedisValue>> newkeyValues =
                keyValues.Select(p => new KeyValuePair<RedisKey, RedisValue>(p.Key, p.Value)).ToList();
            return await Do(db => db.StringSetAsync(newkeyValues.ToArray()));
        }

        /// <summary>保存一个对象</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="secondTimeout"></param>
        /// <returns></returns>
        public static async Task<bool> StringSetAsync<T>(string key, T obj, int secondTimeout)
        {
            string json = ConvertJson(obj);
            return await Do(db => db.StringSetAsync(key, json, TimeSpan.FromSeconds(secondTimeout)));
        }

        /// <summary>获取单个key的值</summary>
        /// <param name="key">Redis Key</param>
        /// <returns></returns>
        public static async Task<string> StringGetAsync(string key)
        {
            return await Do(db => db.StringGetAsync(key));
        }

        /// <summary>获取一个key的对象</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<T> StringGetAsync<T>(string key)
        {
            string result = await Do(db => db.StringGetAsync(key));
            return ConvertObj<T>(result);
        }

        /// <summary>为数字增长val</summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public static async Task<long> StringIncrementAsync(string key, long val = 1)
        {
            return await Do(db => db.StringIncrementAsync(key, val));
        }

        /// <summary>为数字减少val</summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public static async Task<long> StringDecrementAsync(string key, long val = 1)
        {
            return await Do(db => db.StringDecrementAsync(key, val));
        }

        /// <summary>为数字增长val</summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public static async Task<double> StringIncrementAsync(string key, double val = 1)
        {
            return await Do(db => db.StringIncrementAsync(key, val));
        }

        /// <summary>为数字减少val</summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public static async Task<double> StringDecrementAsync(string key, double val = 1)
        {
            return await Do(db => db.StringDecrementAsync(key, val));
        }

        #endregion 异步方法

        #endregion String

        #region Hash

        #region 同步方法

        /// <summary>判断某个数据是否已经被缓存</summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static bool HashExists(string key, string dataKey)
        {
            return Do(db => db.HashExists(key, dataKey));
        }

        /// <summary>存储数据到hash表</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool HashSet<T>(string key, string dataKey, T t)
        {
            return Do(db =>
            {
                string json = ConvertJson(t);
                return db.HashSet(key, dataKey, json);
            });
        }

        /// <summary>移除hash中的某值</summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static bool HashDel(string key, string dataKey)
        {
            return Do(db => db.HashDelete(key, dataKey));
        }

        /// <summary>移除hash中的多个值</summary>
        /// <param name="key"></param>
        /// <param name="dataKeys"></param>
        /// <returns></returns>
        public static long HashDel(string key, List<RedisValue> dataKeys)
        {
            return Do(db => db.HashDelete(key, dataKeys.ToArray()));
        }

        /// <summary>从hash表获取数据</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static T HashGet<T>(string key, string dataKey)
        {
            return Do(db =>
            {
                if (!HashExists(key, dataKey)) return default(T);
                string value = db.HashGet(key, dataKey);
                return ConvertObj<T>(value);
            });
        }

        /// <summary>为数字增长val</summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public static double HashIncrement(string key, string dataKey, double val = 1)
        {
            return Do(db => db.HashIncrement(key, dataKey, val));
        }

        /// <summary>为数字减少val</summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public static double HashDecrement(string key, string dataKey, double val = 1)
        {
            return Do(db => db.HashDecrement(key, dataKey, val));
        }

        /// <summary>获取hashkey所有Redis key</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<T> HashKeys<T>(string key)
        {
            return Do(db =>
            {
                RedisValue[] values = db.HashKeys(key);
                return ConvetList<T>(values);
            });
        }

        /// <summary>获取hashkey所有Redis key</summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> HashGetAll<T>(string key)
        {
            var values = Do(db => db.HashGetAll(key));
            return ConvetList<T>(values);
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>判断某个数据是否已经被缓存</summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static async Task<bool> HashExistsAsync(string key, string dataKey)
        {
            return await Do(db => db.HashExistsAsync(key, dataKey));
        }

        /// <summary>存储数据到hash表</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static async Task<bool> HashSetAsync<T>(string key, string dataKey, T t)
        {
            return await Do(db =>
            {
                string json = ConvertJson(t);
                return db.HashSetAsync(key, dataKey, json);
            });
        }

        /// <summary>移除hash中的某值</summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static async Task<bool> HashDelAsync(string key, string dataKey)
        {
            return await Do(db => db.HashDeleteAsync(key, dataKey));
        }

        /// <summary>移除hash中的多个值</summary>
        /// <param name="key"></param>
        /// <param name="dataKeys"></param>
        /// <returns></returns>
        public static async Task<long> HashDelAsync(string key, List<RedisValue> dataKeys)
        {
            return await Do(db => db.HashDeleteAsync(key, dataKeys.ToArray()));
        }

        /// <summary>从hash表获取数据</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static async Task<T> HashGeAsync<T>(string key, string dataKey)
        {
            string value = await Do(db => db.HashGetAsync(key, dataKey));
            return ConvertObj<T>(value);
        }

        /// <summary>为数字增长val</summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public static async Task<double> HashIncrementAsync(string key, string dataKey, double val = 1)
        {
            return await Do(db => db.HashIncrementAsync(key, dataKey, val));
        }

        /// <summary>为数字减少val</summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public static async Task<long> HashDecrementAsync(string key, string dataKey, long val = 1)
        {
            return await Do(db => db.HashDecrementAsync(key, dataKey, val));
        }

        /// <summary>为数字增长val</summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public static async Task<long> HashIncrementAsync(string key, string dataKey, long val = 1)
        {
            return await Do(db => db.HashIncrementAsync(key, dataKey, val));
        }

        /// <summary>为数字减少val</summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public static async Task<double> HashDecrementAsync(string key, string dataKey, double val = 1)
        {
            return await Do(db => db.HashDecrementAsync(key, dataKey, val));
        }

        /// <summary>获取hashkey所有Redis key</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<List<T>> HashKeysAsync<T>(string key)
        {
            RedisValue[] values = await Do(db => db.HashKeysAsync(key));
            return ConvetList<T>(values);
        }

        /// <summary>获取hashkey所有Redis key</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<List<T>> HashGetAllAsync<T>(string key)
        {
            var values = await Do(db => db.HashGetAllAsync(key));
            return ConvetList<T>(values);
        }

        #endregion 异步方法

        #endregion Hash

        #region List

        #region 同步方法

        /// <summary>移除指定ListId的内部List的值</summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void ListRemove<T>(string key, T value)
        {
            Do(db => db.ListRemove(key, ConvertJson(value)));
        }

        /// <summary>获取指定key的List</summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<T> ListRange<T>(string key)
        {
            return Do(redis =>
            {
                var values = redis.ListRange(key);
                return ConvetList<T>(values);
            });
        }

        /// <summary>入队</summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void ListRightPush<T>(string key, T value)
        {
            Do(db => db.ListRightPush(key, ConvertJson(value)));
        }

        /// <summary>出队</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T ListRightPop<T>(string key)
        {
            return Do(db =>
            {
                var value = db.ListRightPop(key);
                return ConvertObj<T>(value);
            });
        }

        /// <summary>入栈</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void ListLeftPush<T>(string key, T value)
        {
            Do(db => db.ListLeftPush(key, ConvertJson(value)));
        }

        /// <summary>出栈</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T ListLeftPop<T>(string key)
        {
            return Do(db =>
            {
                var value = db.ListLeftPop(key);
                return ConvertObj<T>(value);
            });
        }

        /// <summary>获取集合中的数量</summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long ListLength(string key)
        {
            return Do(redis => redis.ListLength(key));
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>移除指定ListId的内部List的值</summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static async Task<long> ListRemoveAsync<T>(string key, T value)
        {
            return await Do(db => db.ListRemoveAsync(key, ConvertJson(value)));
        }

        /// <summary>获取指定key的List</summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<List<T>> ListRangeAsync<T>(string key)
        {
            var values = await Do(redis => redis.ListRangeAsync(key));
            return ConvetList<T>(values);
        }

        /// <summary>入队</summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static async Task<long> ListRightPushAsync<T>(string key, T value)
        {
            return await Do(db => db.ListRightPushAsync(key, ConvertJson(value)));
        }

        /// <summary>出队</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<T> ListRightPopAsync<T>(string key)
        {
            var value = await Do(db => db.ListRightPopAsync(key));
            return ConvertObj<T>(value);
        }

        /// <summary>入栈</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static async Task<long> ListLeftPushAsync<T>(string key, T value)
        {
            return await Do(db => db.ListLeftPushAsync(key, ConvertJson(value)));
        }

        /// <summary>出栈</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<T> ListLeftPopAsync<T>(string key)
        {
            var value = await Do(db => db.ListLeftPopAsync(key));
            return ConvertObj<T>(value);
        }

        /// <summary>获取集合中的数量</summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<long> ListLengthAsync(string key)
        {
            return await Do(redis => redis.ListLengthAsync(key));
        }

        #endregion 异步方法

        #endregion List

        #region SortedSet 有序集合

        #region 同步方法

        /// <summary>添加</summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="score"></param>
        public static bool SortedSetAdd<T>(string key, T value, double score)
        {
            return Do(redis => redis.SortedSetAdd(key, ConvertJson<T>(value), score));
        }

        /// <summary>删除</summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static bool SortedSetRemove<T>(string key, T value)
        {
            return Do(redis => redis.SortedSetRemove(key, ConvertJson(value)));
        }

        /// <summary>获取全部</summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<T> SortedSetRangeByRank<T>(string key)
        {
            return Do(redis =>
            {
                var values = redis.SortedSetRangeByRank(key);
                return ConvetList<T>(values);
            });
        }

        /// <summary>获取集合中的数量</summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long SortedSetLength(string key)
        {
            return Do(redis => redis.SortedSetLength(key));
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>添加</summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="score"></param>
        public static async Task<bool> SortedSetAddAsync<T>(string key, T value, double score)
        {
            return await Do(redis => redis.SortedSetAddAsync(key, ConvertJson<T>(value), score));
        }

        /// <summary>删除</summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static async Task<bool> SortedSetRemoveAsync<T>(string key, T value)
        {
            return await Do(redis => redis.SortedSetRemoveAsync(key, ConvertJson(value)));
        }

        /// <summary>获取全部</summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<List<T>> SortedSetRangeByRankAsync<T>(string key)
        {
            var values = await Do(redis => redis.SortedSetRangeByRankAsync(key));
            return ConvetList<T>(values);
        }

        /// <summary>获取集合中的数量</summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<long> SortedSetLengthAsync(string key)
        {
            return await Do(redis => redis.SortedSetLengthAsync(key));
        }

        #endregion 异步方法

        #endregion SortedSet 有序集合

        #region key

        /// <summary>删除单个key</summary>
        /// <param name="key">redis key</param>
        /// <returns>是否删除成功</returns>
        public static bool KeyDel(string key)
        {
            return Do(db => db.KeyDelete(key));
        }

        /// <summary>删除多个key</summary>
        /// <param name="keys">rediskey</param>
        /// <returns>成功删除的个数</returns>
        public static long KeyDel(List<string> keys)
        {
            return Do(db => db.KeyDelete(ConvertRedisKeys(keys)));
        }

        /// <summary>判断key是否存储</summary>
        /// <param name="key">redis key</param>
        /// <returns></returns>
        public static bool KeyExists(string key)
        {
            return Do(db => db.KeyExists(key));
        }

        /// <summary>设置Key的时间</summary>
        /// <param name="key">redis key</param>
        /// <param name="secondTimeout"></param>
        /// <returns></returns>
        public static bool KeyExpire(string key, int secondTimeout)
        {
            return Do(db => db.KeyExpire(key, TimeSpan.FromSeconds(secondTimeout)));
        }

        #endregion key

        #region 辅助方法

        private static T Do<T>(Func<IDatabase, T> func)
        {
            try
            {
                var database = GetDatabase();
                return func(database);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        private static string ConvertJson<T>(T value)
        {
            string result = value is string ? value.ToString() : JsonConvert.SerializeObject(value);
            return result;
        }

        private static T ConvertObj<T>(RedisValue value)
        {
            if (value.IsNullOrEmpty)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(value.ToString());
        }

        private static List<T> ConvetList<T>(RedisValue[] values)
        {
            List<T> result = new List<T>();
            foreach (var item in values)
            {
                var model = ConvertObj<T>(item);
                result.Add(model);
            }
            return result;
        }

        private static RedisKey[] ConvertRedisKeys(List<string> redisKeys)
        {
            return redisKeys.Select(redisKey => (RedisKey)redisKey).ToArray();
        }

        private static List<T> ConvetList<T>(HashEntry[] values)
        {
            List<T> result = new List<T>();
            foreach (var item in values)
            {
                if (!item.Value.IsNullOrEmpty)
                {
                    var model = ConvertObj<T>(item.Value);
                    result.Add(model);
                }
            }
            return result;
        }

        #endregion 辅助方法

        #region 发布订阅

        /// <summary>发布订阅客户端</summary>
        public static ISubscriber PubSubClient { get => Connection.GetSubscriber(); }

        /// <summary>发布</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        public static void Publish<T>(string channel, T msg)
        {
            PubSubClient.Publish(channel, JsonHelper.ToJson(msg));
        }

        /// <summary>发布</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        public static Task PublishAsync<T>(string channel, T msg)
        {
            return PubSubClient.PublishAsync(channel, JsonHelper.ToJson(msg));
        }

        /// <summary>订阅</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        public static void Subscribe<T>(string channel, Action<T> action)
        {
            PubSubClient.Subscribe(channel, (c, t) => action.Invoke(JsonHelper.FromJson<T>(t)));
        }

        /// <summary>订阅</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        public static Task SubscribeAsync<T>(string channel, Action<T> action)
        {
            return PubSubClient.SubscribeAsync(channel, (c, t) => action.Invoke(JsonHelper.FromJson<T>(t)));
        }

        #endregion 发布订阅

        public static IBatch CreateBatch()
        {
            return GetDatabase().CreateBatch();
        }

        public static ITransaction CreateTransaction()
        {
            return GetDatabase().CreateTransaction();
        }

        //使用Keys *模糊匹配Key
        public static List<string> GetMatchKeys(string key)
        {
            var result = (string[])GetDatabase().ScriptEvaluate(LuaScript.Prepare("return redis.call('KEYS',@keypattern)"), new { keypattern = key });
            return result.ToList();
        }

        //使用SCAN模糊匹配Key
        public static List<string> ScanMatchKeys(string key)
        {
            var result = (string[])GetDatabase().ScriptEvaluate(
                LuaScript.Prepare("local dbsize=redis.call('dbsize') local res=redis.call('scan',0,'match',KEYS[1],'count',dbsize) return res[2]"),
                new RedisKey[] { key });
            return result.ToList();
        }
    }
}