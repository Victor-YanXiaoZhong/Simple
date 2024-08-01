using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;

namespace Simple.Dapper
{
    /// <summary>类型映射帮助类</summary>
    public static class ClassMapHelper
    {
        #region 局部变量

        private static readonly string typeString = typeof(string).FullName;
        private static readonly String typeInt64 = typeof(Int64).FullName;
        private static readonly Type typeIEnumerable = typeof(System.Collections.IEnumerable);

        #endregion 局部变量

        /// <summary>模型缓存</summary>
        private static readonly ConcurrentDictionary<string, ClassMap> classMapCache = new ConcurrentDictionary<string, ClassMap>(StringComparer.OrdinalIgnoreCase);

        private static readonly Dictionary<String, DbType> dbTypes = new Dictionary<String, DbType>(28);

        static ClassMapHelper()
        {
            dbTypes[typeString] = DbType.String;
            dbTypes[typeof(Int32).FullName] = DbType.Int32;
            dbTypes[typeof(UInt32).FullName] = DbType.UInt32;
            dbTypes[typeof(Decimal).FullName] = DbType.Decimal;
            dbTypes[typeof(Double).FullName] = DbType.Double;
            dbTypes[typeof(Single).FullName] = DbType.Single;
            dbTypes[typeof(DateTime).FullName] = DbType.DateTime;
            dbTypes[typeof(Boolean).FullName] = DbType.Boolean;
            dbTypes[typeInt64] = DbType.Int64;
            dbTypes[typeof(UInt64).FullName] = DbType.UInt64;
            dbTypes[typeof(Int16).FullName] = DbType.Int16;
            dbTypes[typeof(UInt16).FullName] = DbType.UInt16;
            dbTypes[typeof(Byte).FullName] = DbType.Byte;
            dbTypes[typeof(SByte).FullName] = DbType.SByte;
            dbTypes[typeof(Byte[]).FullName] = DbType.Binary;
            dbTypes[typeof(Guid).FullName] = DbType.Guid;
            dbTypes[typeof(DateTimeOffset).FullName] = DbType.DateTimeOffset;
        }

        private static ClassMap GenerateClassMap(Type type)
        {
            var table = GetTableAttribute(type);
            var columns = GetDbColumns(type, table.Name, out DbColumnAttribute key);
            var joinQuerys = GetJoinQuerys(type);
            var classMap = new ClassMap(type, table, columns, joinQuerys)
            {
                HasKey = key != null,
                Key = key,
            };
            return classMap;
        }

        /// <summary>获取table标签</summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static DbTableAttribute GetTableAttribute(Type type)
        {
            DbTableAttribute dbTable;
            if (Attribute.GetCustomAttribute(type, typeof(DbTableAttribute)) is DbTableAttribute dbAttribute)
            {
                return dbAttribute;
            }
            if (Attribute.GetCustomAttribute(type, typeof(TableAttribute)) is TableAttribute attribute)
            {
                dbTable = new DbTableAttribute(attribute);
            }
            else
            {
                dbTable = new DbTableAttribute(type.Name);
            }
            return dbTable;
        }

        /// <summary>获取列属性</summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        private static DbColumnAttribute GetDbColumnAttribute(PropertyInfo propertyInfo)
        {
            Type proType = propertyInfo.PropertyType;
            bool isString = string.Equals(typeof(string).FullName, proType.FullName, StringComparison.OrdinalIgnoreCase);
            if (!isString && typeIEnumerable.IsAssignableFrom(proType)) return null; //忽略集合

            if (Attribute.GetCustomAttribute(propertyInfo, typeof(DbColumnAttribute)) is DbColumnAttribute dbColumn)
            {
                if (dbColumn.Name.IsNullOrEmpty())
                    dbColumn.Name = propertyInfo.Name;
            }
            else
            {
                dbColumn = new DbColumnAttribute(propertyInfo.Name, "");
            }

            dbColumn.IsString = isString;
            dbColumn.PropertyName = propertyInfo.Name;
            dbColumn.PropertyInfo = propertyInfo;

            if (!propertyInfo.CanRead) dbColumn.CanExport = false;
            if (!propertyInfo.CanWrite) dbColumn.Ignored = true;

            if (dbColumn.Ignored)
            {
                dbColumn.CanSelect = dbColumn.CanInsert = dbColumn.CanExport = dbColumn.CanUpdate = false;
            }
            else
            {
                if (!dbColumn.DataType.HasValue)
                {
                    dbColumn.DataType = GetDbType(proType);
                }
            }

            return dbColumn;
        }

        /// <summary>获取模型的列</summary>
        /// <param name="type"></param>
        /// <param name="table"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="FatalException"></exception>
        internal static Dictionary<string, DbColumnAttribute> GetDbColumns(Type type, string table, out DbColumnAttribute key)
        {
            key = null;

            var columns = new Dictionary<string, DbColumnAttribute>(StringComparer.OrdinalIgnoreCase);
            var bindingAttr = BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;
            foreach (PropertyInfo propertyInfo in type.GetProperties(bindingAttr))
            {
                var column = GetDbColumnAttribute(propertyInfo);
                if (column == null || column.Ignored) continue;

                if (column.IsKey || column.IsIdentityKey)
                {
                    column.IsKey = true;
                    if (key != null)
                        throw new FatalException($"类型 {type.FullName} 不支持定义多个主键key");

                    key = column;
                }

                columns[propertyInfo.Name] = column;
            }
            return columns;
        }

        /// <summary>获取模型的关联表</summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="FatalException"></exception>
        internal static string GetJoinQuerys(Type type)
        {
            var joins = new StringBuilder();

            var bindingAttr = BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;

            foreach (DbJoinQueryAttribute joinAttr in type.GetCustomAttributes(typeof(DbJoinQueryAttribute), true))
            {
                joins.AppendLine(joinAttr.Value);
            }
            return joins.ToString();
        }

        public static ClassMap GetMap<T>()
        {
            return GetMap(typeof(T));
        }

        public static ClassMap GetMap(Type type)
        {
            if (type.IsInterface)
            {
                throw new FatalException($@"获取映射类型出错，泛型 T {type.FullName} 不能是接口");
            }
            var key = type.FullName;
            try
            {
                if (!classMapCache.TryGetValue(key, out ClassMap classMap))
                {
                    classMap = GenerateClassMap(type);
                    classMapCache[key] = classMap;
                    return classMap;
                }
                return classMap;
            }
            catch (Exception ex)
            {
                throw new FatalException($"{key} 获取模型映射异常", ex);
            }
        }

        #region 数据类型

        private static DbType GetDbType(Type type)
        {
            String fullName;
            if (type.IsValueType)
            {
                if (type.IsEnum)
                {
                    fullName = Enum.GetUnderlyingType(type).FullName;
                }
                else if (String.Equals("Nullable`1", type.Name, StringComparison.OrdinalIgnoreCase))
                {
                    Type srcType = Nullable.GetUnderlyingType(type);
                    fullName = srcType.IsEnum
                        ? Enum.GetUnderlyingType(srcType).FullName
                        : srcType.FullName;
                }
                else
                {
                    fullName = type.FullName;
                }
            }
            else
            {
                fullName = type.FullName;
            }
            if (!dbTypes.TryGetValue(fullName, out DbType dbType))
            {
                dbType = DbType.Object;
            }
            return dbType;
        }

        #endregion 数据类型
    }

    /// <summary>类型映射</summary>
    public class ClassMap
    {
        #region 属性

        public string TableName { get; set; }
        public string Schema { get; set; }
        public DbColumnAttribute Key { get; set; }
        public bool HasKey { get; set; } = false;
        public readonly string joinQuerys;

        #endregion 属性

        #region 私有属性

        private readonly Type classType;
        private readonly DbTableAttribute table;
        private readonly Dictionary<string, DbColumnAttribute> columns;

        #endregion 私有属性

        internal ClassMap(Type type, DbTableAttribute table, Dictionary<string, DbColumnAttribute> columns, string joinQuerys = "")
        {
            if (type.IsInterface)
            {
                throw new FatalException($@"获取映射类型出错，泛型 T {type.FullName} 不能是接口");
            }
            classType = type;
            this.table = table;
            this.columns = columns;
            this.joinQuerys = joinQuerys;
            TableName = table.Name;
            Schema = table.Schema;
        }

        /// <summary>遍历</summary>
        /// <param name="func">处理函数&lt;列, 分隔符&gt;，如果是满足条件的第一项，分隔符返回null</param>
        /// <param name="joiner">连接符</param>
        public void Each(Func<DbColumnAttribute, String, Boolean> func, String joiner = ",")
        {
            using (var en = columns.GetEnumerator())
            {
                // 满足条件的第一项不添加分隔符
                while (en.MoveNext()) { if (func(en.Current.Value, null)) { break; } }
                while (en.MoveNext()) { func(en.Current.Value, joiner); }
            }
        }

        /// <summary>遍历</summary>
        /// <param name="func">处理函数&lt;列, 分隔符&gt;，如果是满足条件的第一项，分隔符返回null</param>
        public void Each(Action<DbColumnAttribute> func)
        {
            using (var en = columns.GetEnumerator())
            {
                while (en.MoveNext()) { func(en.Current.Value); }
            }
        }

        public bool TryGetColumn(string propertyName, out DbColumnAttribute dbColumn)
        {
            return columns.TryGetValue(propertyName, out dbColumn);
        }

        /// <summary>获取主键条件SQL，例如 Id=@Id</summary>
        /// <returns></returns>
        public string TryGetIdWhere(ISQLGenerator sqlGenerator)
        {
            if (!HasKey || Key == null) throw new FatalException($"类型 {TableName} 不存在主键");

            return $" {Key.Name} = {sqlGenerator.ParameterPrefix}{Key.Name}";
        }

        /// <summary>获取SQL参数</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public IDictionary<string, object> GetParameters<T>(T model)
        {
            var parameters = new Dictionary<string, object>();
            using (var en = columns.GetEnumerator())
            {
                while (en.MoveNext())
                {
                    var column = en.Current.Value;

                    object value = column.GetPropertyValue(model);
                    parameters.Add(column.Name, value);
                }
            }
            return parameters;
        }

        /// <summary>获取SELECT 列</summary>
        /// <param name="sqlGenerator"></param>
        /// <returns></returns>
        public string GetSelectColumns(ISQLGenerator sqlGenerator)
        {
            var list = new List<string>();
            using (var en = columns.GetEnumerator())
            {
                while (en.MoveNext())
                {
                    var column = en.Current.Value;
                    if (!column.CanSelect) continue;

                    var columnTableName = column.Table.IsNotEmpty() ? column.Table + "." : "";
                    if (joinQuerys.IsNullOrEmpty()) columnTableName = "";

                    if (column.Name.ToUpper() != column.PropertyName.ToUpper() || column.Table.IsNotEmpty())
                    {
                        list.Add($" {columnTableName}{sqlGenerator.ColumnDot}{column.Name}{sqlGenerator.ColumnDot} AS {column.PropertyName}");
                    }
                    else
                    {
                        list.Add($" {columnTableName}{sqlGenerator.ColumnDot}{column.Name}{sqlGenerator.ColumnDot}");
                    }
                }
            }
            return string.Join(",", list);
        }

        /// <summary>获取 Insert 列</summary>
        /// <param name="sqlGenerator"></param>
        /// <returns></returns>
        public string GetInsertColumns(ISQLGenerator sqlGenerator, out string parameter)
        {
            var list = new List<string>();
            var parameters = new List<string>();
            using (var en = columns.GetEnumerator())
            {
                while (en.MoveNext())
                {
                    var column = en.Current.Value;
                    if (!column.CanInsert || column.IsIdentityKey) continue;

                    list.Add($" {sqlGenerator.ColumnDot}{column.Name}{sqlGenerator.ColumnDot}");
                    parameters.Add($" {sqlGenerator.ParameterPrefix}{column.Name} ");
                }
            }
            parameter = string.Join(",", parameters);
            return string.Join(",", list);
        }

        /// <summary>获取 Update 列</summary>
        /// <param name="sqlGenerator"></param>
        /// <returns></returns>
        public string GetUpdateColumns(ISQLGenerator sqlGenerator)
        {
            var list = new List<string>();
            using (var en = columns.GetEnumerator())
            {
                while (en.MoveNext())
                {
                    var column = en.Current.Value;
                    if (!column.CanUpdate || column.IsIdentityKey) continue;

                    list.Add($" {sqlGenerator.ColumnDot}{column.Name}{sqlGenerator.ColumnDot} = {sqlGenerator.ParameterPrefix}{column.Name}");
                }
            }
            return string.Concat(" SET ", string.Join(",", list));
        }
    }
}