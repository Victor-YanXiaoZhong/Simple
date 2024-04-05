using Newtonsoft.Json;
using Simple.Utils.Helper;
using System.Collections.Concurrent;
using System.Configuration;

namespace Simple.Dapper
{
    /// <summary>.NET CORE下使用，需要先注册db工厂 eg: RegistFactoty(SqlClientFactory.Instance)</summary>
    public partial class DbComponent
    {
        /// <summary>获取数据库操作实例</summary>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        /// <exception cref="FatalException"></exception>
        public static DbComponent Instance(string connectionName)
        {
            if (connectionName.IsNullOrEmpty())
                throw new FatalException($"数据库连接名称 {connectionName} 不能为空", "数据库连接失败");
            return new DbComponent(connectionName);
        }

#if !NET47

        /// <summary>.NET CORE下使用，需要先注册db工厂 eg: RegistFactoty("Microsoft.Data.SqlClient",SqlClientFactory.Instance)</summary>
        /// <param name="providerName">驱动名称</param>
        /// <param name="factory">工厂实例</param>
        public static void RegistFactoty(string providerName, DbProviderFactory factory)
        {
            dbfactoryDic[providerName.ToUpper()] = factory;
        }

#endif

        /// <summary>获取数据库操作实例</summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        /// <exception cref="FatalException"></exception>
        public static DbComponent Instance(ConnectionStringSettings connectionString)
        {
            if (connectionString.ProviderName.IsNullOrEmpty())
                throw new FatalException($"数据库连接提供者 {connectionString.ProviderName} 不能为空", "数据库连接失败");
            if (connectionString.ConnectionString.IsNullOrEmpty())
                throw new FatalException($"数据库连接字符串 {connectionString.ConnectionString} 不能为空", "数据库连接失败");
            return new DbComponent(connectionString);
        }

        private DbProviderFactory dbfactory;
        private static ConcurrentDictionary<string, DbProviderFactory> dbfactoryDic = new();

        /// <summary>数据库连接</summary>
        private readonly ConnectionStringSettings connectionSetting;

        private DatabaseType databaseType;

        /// <summary>sql 生成器</summary>
        private ISQLGenerator sqlGenerator;

        /// <summary>超时时间 120 秒</summary>
        private static readonly int timeOutValue = 120;

        private void InitComponent()
        {
#if NET47
            try
            {
                dbfactory = DbProviderFactories.GetFactory(connectionSetting.ProviderName);
            } catch(Exception ex) {
                throw new FatalException("不支持的数据库类型：" + connectionSetting.ProviderName, "不支持的数据库类型", ex);
            }
#else
            try
            {
                var result = dbfactoryDic.TryGetValue(connectionSetting.ProviderName.ToUpper(), out dbfactory);

                if (!result)
                {
                    throw new FatalException($"在Core下使用组件，数据库需要先注册 eg：DbComponent.RegistFactoty('{connectionSetting.ProviderName}',SqlClientFactory.Instance)", "未注册的数据库类型");
                }
            }
            catch (Exception ex)
            {
                throw new FatalException("不支持的数据库类型：" + connectionSetting.ProviderName, "不支持的数据库类型", ex);
            }
#endif

            switch (connectionSetting.ProviderName)
            {
                case "System.Data.SqlClient":
                    databaseType = DatabaseType.SQLServer;
                    break;

                case "Microsoft.Data.SqlClient":
                    databaseType = DatabaseType.SQLServer;
                    break;

                case "MySql.Data.MySqlClient":
                    databaseType = DatabaseType.MySql;
                    break;

                case "System.Data.SQLite":
                    databaseType = DatabaseType.SQLite;
                    break;

                case "Microsoft.Data.Sqlite":
                    databaseType = DatabaseType.SQLite;
                    break;

                default:
                    break;
            }
            sqlGenerator = SQLGeneratorBase.GetInstance(databaseType);
        }

        /// <summary>获取新数据库连接，连接成功后打开连接</summary>
        public DbConnection NewConnection
        {
            get
            {
                var connection = dbfactory.CreateConnection();
                connection.ConnectionString = connectionSetting.ConnectionString;
                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {
                    throw new FatalException($"数据库连接 {connectionSetting.ConnectionString} 异常", ex);
                }
                return connection;
            }
        }

        /// <summary>打印执行SQL和参数</summary>
        public static event Action<string, string> SqlPrint;

        /// <summary>根据配置名称获取连接对象</summary>
        /// <param name="connectionStringName"></param>
        private DbComponent(string connectionStringName)
        {
            // 此处是否应该采用多例模式，缓存 providerFactory？ 经测试，5秒内可创建20万个Database对象，不必进行缓存
            var connectionStringSetting = ConfigHelper.GetConnectionStringSetting(connectionStringName);
            connectionSetting = new ConnectionStringSettings
            {
                ProviderName = connectionStringSetting.ProviderName,
                ConnectionString = connectionStringSetting.ConnectionString,
            };

            InitComponent();
        }

        /// <summary>直接设置连接对象</summary>
        /// <param name="connectionString"></param>
        /// <param name="isString"></param>
        private DbComponent(ConnectionStringSettings connectionStringSettings)
        {
            this.connectionSetting = connectionStringSettings;
            InitComponent();
        }

        /// <summary>测试连接</summary>
        /// <returns></returns>
        public bool TestConection()
        {
            return NewConnection.State == ConnectionState.Open;
        }

        /// <summary>命令准备 原始SQL命令准备</summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        protected static IDbCommand PrepareCommand(IDbConnection connection, IDbTransaction transaction, string sql, IEnumerable<DbParameter> parameters, CommandType commandType = CommandType.Text, int? timeOut = null)
        {
            var dbCommand = connection.CreateCommand();
            dbCommand.CommandType = commandType;
            dbCommand.CommandText = sql;
            if (transaction != null) dbCommand.Transaction = transaction;
            if (timeOut != null) dbCommand.CommandTimeout = timeOut.Value;

            var sqlPrint = string.Empty;

            if (parameters != null)
            {
                foreach (DbParameter parameter in parameters)
                {
                    dbCommand.Parameters.Add(parameter);
                    sqlPrint += $"{parameter.ParameterName}={parameter.Value}，";
                }
            }
            SqlPrint?.Invoke(sql, sqlPrint);
            return dbCommand;
        }

        protected static void DisposeCommand(IDbCommand dbCommand)
        {
            if (dbCommand != null) { dbCommand.Parameters.Clear(); dbCommand.Dispose(); }
        }

        /// <summary>关闭连接、事务</summary>
        public static void Close(IDbConnection connection, IDbTransaction? transaction = null, bool isCommit = false)
        {
            try
            {
                if (transaction != null)
                {
                    if (isCommit) { transaction.Commit(); } else { transaction.Rollback(); }
                }
            }
            catch (Exception ex)
            {
                throw new FatalException($"数据库事务处理异常", "操作数据库异常", ex);
            }
            finally
            {
                if (transaction != null) transaction.TryDispose();
                if (connection != null) connection.TryDispose();
            }
        }

        /// <summary>创建参数</summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="parameterDirection"></param>
        /// <returns></returns>
        public DbParameter CreateParameter(string name, object value, ParameterDirection parameterDirection = ParameterDirection.Input)
        {
            DbParameter parameter = dbfactory.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            parameter.Direction = parameterDirection;
            return parameter;
        }

        #region 事务执行

        public bool Transexecute(Func<IDbConnection, IDbTransaction, bool> func)
        {
            if (func == null) throw new ArgumentNullException("事务执行时，执行函数不能为null");

            bool issuccess = false;
            IDbConnection connection = null;
            IDbTransaction transaction = null;
            try
            {
                connection = NewConnection;
                transaction = connection.BeginTransaction();
                issuccess = func(connection, transaction);
            }
            finally
            {
                Close(connection, transaction, issuccess);
            }
            return issuccess;
        }

        #endregion 事务执行

        public string GetVersion()
        {
            var version = sqlGenerator.GetVersion();
            return version;
        }

        /// <summary>处理执行过程的异常</summary>
        /// <param name="commandText"></param>
        /// <param name="param"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static DbFatalException DbException(string commandText, object param = null, Exception ex = null)
        {
            var msg = $"Sql命令执行异常：" +
                $"命令：{commandText}" +
                $"参数：{JsonConvert.SerializeObject(param)}";
            return new DbFatalException(msg, "数据库操作异常", ex);
        }
    }
}