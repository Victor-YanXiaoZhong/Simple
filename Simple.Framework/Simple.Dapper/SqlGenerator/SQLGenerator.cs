using System.Collections.Concurrent;

namespace Simple.Dapper
{
    /// <summary>SQL生成接口</summary>
    public interface ISQLGenerator
    {
        /// <summary>列名左右的标点，比如 [Name] `Name` 默认空 用于处理插入时的问题</summary>
        string ColumnDot { get; }

        /// <summary>参数前缀 @ ：之类</summary>
        string ParameterPrefix { get; set; }

        /// <summary>获取表名</summary>
        /// <param name="classMap"></param>
        /// <returns></returns>
        string GetTableName(ClassMap classMap);

        /// <summary>获取列名组合 Id,Name ...</summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        string GetColumns(ClassMap classMap);

        /// <summary>获取查询最后插入的自增Id的语句</summary>
        /// <returns></returns>
        string GetIdentity();

        /// <summary>获取版本号</summary>
        /// <returns></returns>
        string GetVersion();

        /// <summary>生成Insert</summary>
        /// <param name="classMap"></param>
        /// <returns></returns>
        string Insert(ClassMap classMap);

        /// <summary>获取更新的语句 根据主键更新</summary>
        /// <param name="classMap"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        string Update(ClassMap classMap);

        /// <summary>获取删除的语句 根据主键更新</summary>
        /// <param name="classMap"></param>
        /// <returns></returns>
        string Delete(ClassMap classMap);

        /// <summary>获取where下的数量的语句</summary>
        /// <param name="classMap"></param>
        /// <returns></returns>
        string Count(ClassMap classMap, string @where);

        /// <summary>获取根据条件查询的SQL语句</summary>
        /// <param name="classMap"></param>
        /// <param name="where"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        string Select(ClassMap classMap, string @where = null, string @sort = null);

        string GetParameter(DbColumnAttribute column);

        /// <summary>获取表信息</summary>
        /// <returns></returns>
        string GetTableSchema();

        /// <summary>获取字段信息</summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        string GetColumnSchema(string tableName);
    }

    /// <summary>SQL生成</summary>
    public abstract class SQLGeneratorBase : ISQLGenerator
    {
        #region 属性

        /// <summary>参数前缀</summary>
        public virtual string ParameterPrefix { get; set; } = "@";

        /// <summary>别名字符</summary>
        public virtual string Alias { get; set; } = " AS ";

        /// <summary>列名左右的标点，比如 [Name] `Name` 默认空 用于处理插入时的问题</summary>
        public virtual string ColumnDot { get; } = "";

        #endregion 属性

        /// <summary>数量语句的缓存</summary>
        protected readonly ConcurrentDictionary<string, string> countCache = new ConcurrentDictionary<string, string>();

        /// <summary>删除语句的缓存</summary>
        protected readonly ConcurrentDictionary<string, string> deleteCache = new ConcurrentDictionary<string, string>();

        /// <summary>插入语句的缓存</summary>
        protected readonly ConcurrentDictionary<string, string> insertCache = new ConcurrentDictionary<string, string>();

        /// <summary>查询语句的缓存</summary>
        protected readonly ConcurrentDictionary<string, string> selectCache = new ConcurrentDictionary<string, string>();

        /// <summary>更新语句的缓存</summary>
        protected readonly ConcurrentDictionary<string, string> updateCache = new ConcurrentDictionary<string, string>();

        protected string GetSelectSql(ClassMap classMap)
        {
            return selectCache.GetOrAdd(classMap.TableName, (sql) =>
            {
                var columns = classMap.GetSelectColumns(this);
                return $" SELECT {columns} FROM {classMap.TableName} ";
            });
        }

        public static ISQLGenerator GetInstance(DatabaseType databaseType)
        {
            switch (databaseType)
            {
                case DatabaseType.SQLServer: return new SQLServerGenerator();
                case DatabaseType.MySql: return new MySqlGenerator();
                case DatabaseType.SQLite: return new SQLiteGenerator();
                default: throw new ArgumentException($"不支持的数据库类型：{databaseType}");
            }
        }

        public virtual string GetColumns(ClassMap classMap)
        {
            return classMap.GetSelectColumns(this);
        }

        public virtual string GetColumnName(ClassMap classMap, string propertyName)
        {
            if (classMap.TryGetColumn(propertyName, out DbColumnAttribute column))
                return column.Name;
            throw new FatalException($"实体 {classMap.TableName} 中不存在属性 {propertyName}");
        }

        public virtual string GetIdentity()
        {
            return "SELECT SCOPE_IDENTITY() AS Id";
        }

        public virtual string GetTableName(ClassMap classMap)
        {
            return classMap.TableName;
        }

        public virtual string Count(ClassMap classMap, string where)
        {
            return countCache.GetOrAdd(classMap.TableName, (sql) =>
            {
                return $"SELECT Count(1) FROM {classMap.TableName} WHERE {classMap.TryGetIdWhere(this)};";
            });
        }

        public virtual string Delete(ClassMap classMap)
        {
            return deleteCache.GetOrAdd(classMap.TableName, (sql) =>
            {
                return $"DELETE FROM {classMap.TableName} WHERE {classMap.TryGetIdWhere(this)};";
            });
        }

        /// <summary>获取模型插入语句，同时包含获取自增主键的语句</summary>
        /// <param name="classMap"></param>
        /// <returns></returns>
        public virtual string Insert(ClassMap classMap)
        {
            return insertCache.GetOrAdd(classMap.TableName, (sql) =>
            {
                var columns = classMap.GetInsertColumns(this, out string valueParame);
                return $"INSERT INTO {classMap.TableName} ({columns}) VALUES ({valueParame});{GetIdentity()};";
            });
        }

        public virtual string Select(ClassMap classMap, string where = null, string sort = null)
        {
            var sql = GetSelectSql(classMap);
            return $"{sql} {" WHERE " + (!where.IsNullOrEmpty() ? where : classMap.TryGetIdWhere(this))} {sort};";
        }

        public virtual string Update(ClassMap classMap)
        {
            return updateCache.GetOrAdd(classMap.TableName, (sql) =>
            {
                var columns = classMap.GetUpdateColumns(this);

                if (!classMap.HasKey) throw new FatalException($"类型 {classMap.TableName} 没有主键，不能进行无条件更新操作");

                return $" UPDATE {classMap.TableName} {columns} WHERE {classMap.TryGetIdWhere(this)};";
            });
        }

        public virtual string GetVersion()
        {
            throw new NotImplementedException();
        }

        /// <summary>获取参数</summary>
        /// <param name="column"></param>
        /// <exception cref="NotImplementedException"></exception>
        public string GetParameter(DbColumnAttribute column)
        {
            return String.Concat(column.Name, "=", ParameterPrefix, column.PropertyName);
        }

        /// <summary>获取表信息架构</summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string GetTableSchema()
        {
            return $"SELECT TABLE_NAME AS TableName,TABLE_NAME AS ClassName,'' AS Description FROM INFORMATION_SCHEMA.TABLES;";
        }

        /// <summary>获取字段信息架构</summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string GetColumnSchema(string tableName)
        {
            return $"SELECT A.COLUMN_NAME AS ClassPropName,A.COLUMN_NAME AS DbColumnName,CASE A.is_nullable WHEN 'YES' THEN 1 ELSE 0 END AS Required,\r\n\tA.DATA_TYPE AS DbType,A.ORDINAL_POSITION AS Sort,B.is_identity AS IsIdentity,B.is_identity AS IsPrimaryKey,isnull(g.[value],'') AS Description\r\nFROM\r\n  SYS.COLUMNS B LEFT JOIN SYS.extended_properties G\r\n on (B.object_id = g.major_id AND g.minor_id = B.column_id)\r\n LEFT JOIN INFORMATION_SCHEMA.COLUMNS A ON (A.TABLE_NAME = '{tableName}' AND A.COLUMN_NAME = B.NAME)\r\nWHERE object_id = (SELECT object_id FROM sys.tables WHERE name = '{tableName}')";
        }
    }
}