namespace Simple.Dapper
{
    public partial class DbComponent
    {
        /// <summary>获取插入的参数</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="classMap"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private List<DbParameter> GetInsertDbParams<T>(ClassMap classMap, T entity)
        {
            var parameters = new List<DbParameter>();
            classMap.Each((column, separator) =>
            {
                Object value;
                DbParameter param;
                Boolean flag = column.CanInsert;
                if (flag && !column.IsIdentityKey)
                {
                    value = column.GetPropertyValue(entity);
                    if (value == null)
                    {
                        if (column.IsString) { value = String.Empty; } else { value = DBNull.Value; }
                    }
                    param = CreateParameter($"{sqlGenerator.ParameterPrefix}{column.Name}", value);
                    param.DbType = column.DataType.Value;
                    param.IsNullable = column.IsNullable;
                    parameters.Add(param);
                }
                return flag;
            });
            return parameters;
        }

        /// <summary>插入单条 主键自动赋值</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public T Insert<T>(T entity, DbConnection conn = null, DbTransaction transaction = null)
        {
            bool isSuccess = false;
            var classMap = ClassMapHelper.GetMap<T>();
            var commandText = sqlGenerator.Insert(classMap);
            var param = GetInsertDbParams<T>(classMap, entity);
            try
            {
                if (conn == null) conn = NewConnection;

                var command = PrepareCommand(conn, transaction, commandText, param);
                var keyValue = command.ExecuteScalar();
                isSuccess = keyValue != null;
                if (classMap.Key.DataType.HasValue)
                {
                    switch (classMap.Key.DataType)
                    {
                        case DbType.Int16 | DbType.Int32:
                            isSuccess = Int32.TryParse(keyValue.ToString(), out Int32 int32Key);
                            if (isSuccess) { classMap.Key.SetPropertyValue(entity, int32Key); }
                            break;

                        default:
                            classMap.Key.SetPropertyValue(entity, keyValue);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw DbException(commandText, param, ex);
            }
            finally
            {
                Close(conn, transaction, isSuccess);
            }
            return entity;
        }

        /// <summary>插入多条 主键不自动赋值,批量自动事务</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public T BatchInsert<T>(IEnumerable<T> entities, int? timeOut)
        {
            DbConnection conn = null;
            DbTransaction transaction = null;
            T model = default;
            bool isSuccess = false;
            var classMap = ClassMapHelper.GetMap<T>();
            var commandText = sqlGenerator.Insert(classMap);
            try
            {
                conn = NewConnection;
                transaction = conn.BeginTransaction();
                isSuccess = conn.Execute(commandText, entities, transaction, commandTimeout: timeOut) == entities.Count();
            }
            catch (Exception ex)
            {
                throw DbException(commandText, entities, ex);
            }
            finally
            {
                Close(conn, transaction, isSuccess);
            }
            return model;
        }
    }
}