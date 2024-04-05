using LiteDB;
using Newtonsoft.Json;
using System.Text;

namespace Simple.Utils
{
    /// <summary>APP数据字典帮助类，存储在LiteDB中 类似于字典 db -&gt; table -&gt; (k,v)</summary>
    internal class ApplicationConfig
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public byte[] Value { get; set; }
    }

    /// <summary>LitDb下的一个配置管理 APP数据字典帮助类，存储在LiteDB中 类似于字典 db -&gt; table -&gt; (k,v)</summary>
	public class AppProfileDataDb
    {
        private static AppProfileDataDb appDb;

        private AppProfileDataDb()
        {
            if (ProfileDb == null)
            {
                ProfileDb = new LiteDatabase("profile.db");
            }
        }

        public AppProfileDataDb(string dbName)
        {
            if (ProfileDb == null)
            {
                ProfileDb = new LiteDatabase($"{dbName}.db");
            }
        }

        internal LiteDatabase ProfileDb { get; }

        /// <summary>临时数据的存储表 默认表名称TempData</summary>
        public AppProfileDataTable TempData
        {
            get
            {
                return new AppProfileDataTable(this, "TempData");
            }
        }

        /// <summary>设置新数据数据</summary>
        /// <param name="collection">存储的表名</param>
        /// <returns></returns>
        public AppProfileDataTable this[string collection]
        {
            get
            {
                return new AppProfileDataTable(this, collection);
            }
        }

        /// <summary>获取实例 无db名称 默认使用profile</summary>
        /// <returns></returns>
        public static AppProfileDataDb GetInstance(string dbName = "")
        {
            if (appDb == null)
            {
                if (dbName.IsNotEmpty())
                {
                    appDb = new AppProfileDataDb(dbName);
                }
                else
                {
                    appDb = new AppProfileDataDb();
                }
            }
            return appDb;
        }
    }

    /// <summary>程序数据字典表 注意内部Key不要重复</summary>
    public class AppProfileDataTable
    {
        private ILiteCollection<ApplicationConfig> collection;

        internal AppProfileDataTable(AppProfileDataDb appdb, string collectonName)
        {
            collection = appdb.ProfileDb.GetCollection<ApplicationConfig>(collectonName);
        }

        /// <summary>设置表字段数据</summary>
        /// <param name="key">字段名称</param>
        /// <param name="value">字段值</param>
        public void Set(string key, object value)
        {
            var json = JsonConvert.SerializeObject(value);
            var buff = Encoding.UTF8.GetBytes(json);

            var kv = collection.FindOne(x => x.Key == key);
            if (kv == null)
            {
                kv = new ApplicationConfig
                {
                    Key = key
                };

                collection.EnsureIndex(x => x.Key);
            }

            kv.Value = buff;

            collection.Upsert(kv);
        }

        /// <summary>获取字典key值</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T? Get<T>(string key)
        {
            var kv = collection.FindOne(x => x.Key == key);
            if (kv == null)
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(kv.Value));
        }
    }
}