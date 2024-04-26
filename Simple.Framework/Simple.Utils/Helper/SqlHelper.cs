using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Collections.Concurrent;
using System.Configuration;
using LiteDB;
using System.Data;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;
using System.Transactions;
using Newtonsoft.Json.Linq;
using NLog.Config;

namespace Simple.Utils.Helper
{
    public partial class SqlHelper
    {
        /// <summary>测试连接</summary>
        /// <returns></returns>
        public bool TestConection()
        {
            using (var connection = NewConnection)
            {
                return connection.State == ConnectionState.Open;
            }
        }

        /// <summary>获取连接版本</summary>
        /// <returns></returns>
        public string ServerVersion()
        {
            using (var connection = NewConnection)
            {
                return connection.ServerVersion;
            }
        }

        #region 查询

        /// <summary>执行查询语句，返回结果集</summary>
        public List<T> GetList<T>(string commandText, CommandType commandType = CommandType.Text,
            object? parameters = null, int? timeOut = null) where T : class, new()
        {
            return Execute(command =>
            {
                var reader = command.ExecuteReader();
                return reader.ToList<T>();
            }, commandText, CreateParameters(parameters), commandType, false, timeOut);
        }

        /// <summary>执行查询语句，返回单个结果 如果有多个结果，默认返回第一个，没有结果返回null</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public T FirstOrDefault<T>(string commandText, CommandType commandType = CommandType.Text,
            object? parameters = null, int? timeOut = null) where T : class, new()
        {
            return Execute(command =>
            {
                var reader = command.ExecuteReader();
                return reader.ToEntity<T>();
            }, commandText, CreateParameters(parameters), commandType, false, timeOut);
        }

        #endregion 查询

        #region 执行

        /// <summary>执行SQL语句，返回受影响的行数</summary>
        public int ExecuteNonQuery(string commandText, CommandType commandType = CommandType.Text,
            object? parameters = null, bool useTransaction = false, int? timeOut = null)
        {
            return Execute(command =>
            {
                return command.ExecuteNonQuery();
            }, commandText, CreateParameters(parameters), commandType, useTransaction, timeOut);
        }

        /// <summary>执行SQL语句，返回单个值</summary>
        public T ExecuteScalar<T>(string commandText, CommandType commandType = CommandType.Text,
            object? parameters = null, int? timeOut = null)
        {
            return Execute(command =>
            {
                return (T)(command.ExecuteScalar());
            }, commandText, CreateParameters(parameters), commandType, false, timeOut);
        }

        #endregion 执行
    }

    /// <summary>SqlHelper 类，封装了数据库连接、执行SQL语句等功能</summary>
    public partial class SqlHelper : IDisposable
    {
        private static ConcurrentDictionary<string, DbProviderFactory> dbfactoryDic = new();

        /// <summary>数据库连接</summary>
        private ConnectionStringSettings connectionSetting;

        private DbProviderFactory? dbfactory;

        /// <summary>传入数据库连接字符串名称，初始化数据库连接</summary>
        /// <param name="connectionSetting"></param>
        public SqlHelper(ConnectionStringSettings connectionSetting)
        {
            this.connectionSetting = connectionSetting;
            InitComponent();
        }

        /// <summary>传入数据库连接字符串名称，初始化数据库连接</summary>
        /// <param name="connectionStringName"></param>
        public SqlHelper(string connectionStringName)
        {
            var connectionStringSetting = ConfigHelper.GetConnectionStringSetting(connectionStringName);
            connectionSetting = new ConnectionStringSettings
            {
                ProviderName = connectionStringSetting.ProviderName,
                ConnectionString = connectionStringSetting.ConnectionString,
            };
            InitComponent();
        }

        /// <summary>打印执行SQL和参数</summary>
        public static event Action<string, string> SqlPrint;

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

        private void InitComponent()
        {
            if (this.connectionSetting.ProviderName.IsNullOrEmpty())
                throw new FatalException($"数据库连接提供者 {this.connectionSetting.ProviderName} 不能为空", "数据库连接失败");
            if (this.connectionSetting.ConnectionString.IsNullOrEmpty())
                throw new FatalException($"数据库连接字符串 {this.connectionSetting.ConnectionString} 不能为空", "数据库连接失败");
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
            if (dbfactory is null)
            {
                throw new FatalException("不支持的数据库类型：" + connectionSetting.ProviderName + "未能获取到 dbfactory", "不支持的数据库类型");
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (dbfactory != null)
                {
                    dbfactory = null;
                }
            }
        }

        /// <summary>.NET CORE下使用，需要先注册db工厂 eg: RegistFactoty("Microsoft.Data.SqlClient",SqlClientFactory.Instance)</summary>
        /// <param name="providerName">驱动名称</param>
        /// <param name="factory">工厂实例</param>
        public static void RegistFactoty(string providerName, DbProviderFactory factory)
        {
            dbfactoryDic[providerName.ToUpper()] = factory;
        }

        /// <summary>创建参数集合</summary>
        /// <param name="parameters">new {id=1, name="张三", ...}这样的匿名类型传递SQL参数</param>
        public IEnumerable<DbParameter> CreateParameters(object? parameters)
        {
            // 检查参数对象是否为null
            if (parameters == null) return null;

            var result = new List<DbParameter>();
            // 使用反射获取所有的属性
            var props = parameters.GetType().GetProperties();
            foreach (var prop in props)
            {
                // 创建一个新的参数
                var param = dbfactory.CreateParameter();
                param.ParameterName = "@" + prop.Name;
                param.Value = prop.GetValue(parameters) ?? DBNull.Value; // 如果属性值为null，则使用DBNull.Value

                // 将参数添加到命令对象中
                result.Add(param);
            }
            return result;
        }

        /// <summary>SQL命令执行方法</summary>
        /// <typeparam name="Tout"></typeparam>
        /// <param name="commandToExecute"></param>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <param name="useTransaction"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public Tout Execute<Tout>(Func<DbCommand, Tout> commandToExecute, string commandText, IEnumerable<DbParameter>? parameters = null,
            CommandType commandType = CommandType.Text, bool useTransaction = false, int? timeOut = null)
        {
            Tout result = default;
            using (var connection = NewConnection)
            {
                using (DbCommand command = dbfactory.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandType = commandType;
                    command.CommandText = commandText;

                    if (timeOut != null) command.CommandTimeout = timeOut.Value;

                    var sqlPrint = string.Empty;

                    if (parameters != null)
                    {
                        foreach (DbParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                            sqlPrint += $"{parameter.ParameterName}={parameter.Value}，";
                        }

                        SqlPrint?.Invoke(commandText, sqlPrint);
                    }

                    SqlPrint?.Invoke(commandText, sqlPrint);
                    if (useTransaction)
                    {
                        using (DbTransaction transaction = connection.BeginTransaction())
                        {
                            command.Transaction = transaction;
                            try
                            {
                                result = commandToExecute(command);
                            }
                            catch (DbException ex)
                            {
                                transaction.Rollback();
                                var sqlDebug = $"执行的Sql：{commandText}，参数：{sqlPrint}";
                                throw new FatalException($"数据库事务处理异常" + sqlDebug, "操作数据库异常", ex);
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            result = commandToExecute(command);
                        }
                        catch (DbException ex)
                        {
                            var sqlDebug = $"执行的Sql：{commandText}，参数：{sqlPrint}";
                            throw new FatalException($"数据库命令处理异常" + sqlDebug, "操作数据库异常", ex);
                        }
                    }
                }
            }
            return result;
        }

        public DataTable ExecuteDataTable(string commandText, CommandType commandType = CommandType.Text, object? parameters = null, int? timeOut = null)
        {
            using (var connection = NewConnection)
            using (DbDataAdapter dataAdapter = dbfactory.CreateDataAdapter())
            using (dataAdapter.SelectCommand = dbfactory.CreateCommand())
            {
                dataAdapter.SelectCommand.Connection = connection;
                dataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                dataAdapter.SelectCommand.CommandType = commandType;
                dataAdapter.SelectCommand.CommandText = commandText;
                if (timeOut.HasValue)
                    dataAdapter.SelectCommand.CommandTimeout = timeOut.Value;

                if (parameters != null)
                {
                    var tmp = CreateParameters(parameters);
                    foreach (DbParameter parameter in tmp)
                    {
                        dataAdapter.SelectCommand.Parameters.Add(parameter);
                    }
                }
                var dataTable = new DataTable("SelectTable");
                dataAdapter.Fill(dataTable);
                return dataTable;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}