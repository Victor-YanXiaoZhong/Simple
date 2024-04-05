using Newtonsoft.Json;
using Simple.Utils.Helper;

namespace Simple.Dapper
{
    public partial class DbComponent
    {
        /// <summary>根据主键及特定条件更新（受影响行数大于0视为操作成功）</summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="connection">数据库连接</param>
        /// <param name="entity">实体</param>
        /// <param name="transaction">事务</param>
        /// <param name="timeout">执行时间</param>
        /// <exception cref="DbOpenException">连接异常</exception>
        /// <exception cref="FatalException">执行异常</exception>
        /// <returns>true：更新成功、false：更新失败</returns>
        public bool Update<T>(T entity, DbConnection connection = null, DbTransaction transaction = null, int? timeout = null
        ) where T : class
        {
            var classMap = ClassMapHelper.GetMap<T>();
            var updateSql = sqlGenerator.Update(classMap);
            DynamicParameters parameters = new DynamicParameters();
            classMap.Each((column) =>
            {
                if (column.CanUpdate)
                {
                    object value = column.PropertyInfo.GetValue(entity, null);
                    parameters.Add($"{sqlGenerator.ParameterPrefix}{column.Name}", value);
                }
            });
            try
            {
                if (connection == null)
                    connection = NewConnection;
                return connection.Execute(updateSql, parameters, transaction, timeout ?? timeOutValue,
                    CommandType.Text) > 0;
            }
            catch (Exception ex) { throw DbException(updateSql, parameters, ex); }
            finally { SqlPrint?.Invoke(updateSql, JsonConvert.SerializeObject(parameters)); }
        }

        /// <summary>根据条件和值，更新数据库表 返回受影响的行数</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="updateObject"></param>
        /// <param name="whereObject"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public int Update<T>(object updateObject, object whereObject, DbConnection connection = null, DbTransaction transaction = null, int? timeout = null
        ) where T : class
        {
            var classMap = ClassMapHelper.GetMap<T>();
            var updateDic = ReflectionHelper.GetDictionary(updateObject);
            var whereDic = ReflectionHelper.GetDictionary(whereObject);
            DynamicParameters parameters = new DynamicParameters();

            var updateFields = new List<string>();
            updateDic.Each((column) =>
            {
                updateFields.Add($"{column.Key}={sqlGenerator.ParameterPrefix}{column.Key}");
                parameters.Add($"{sqlGenerator.ParameterPrefix}{column.Key}", column.Value);
            });

            var whereFields = new List<string>();
            whereDic.Each((column) =>
            {
                whereFields.Add($"{column.Key}={sqlGenerator.ParameterPrefix}{column.Key}");
                parameters.Add($"{sqlGenerator.ParameterPrefix}{column.Key}", column.Value);
            });

            var sql = $"UPDATE {classMap.TableName} SET {string.Join(",", updateFields)} WHERE {string.Join(",", whereFields)} ;";
            try
            {
                if (connection == null)
                    connection = NewConnection;
                return connection.Execute(sql, parameters, transaction, timeout ?? timeOutValue,
                    CommandType.Text);
            }
            catch (Exception ex) { throw DbException(sql, parameters, ex); }
            finally { SqlPrint?.Invoke(sql, JsonConvert.SerializeObject(parameters)); }
        }

        /// <summary>更新指定列</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="updateField"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool UpdateFields<T>(T entity, IEnumerable<string> updateField = null, DbConnection connection = null, DbTransaction transaction = null, int? timeout = null) where T : class
        {
            if (updateField == null)
            {
                return Update<T>(entity, connection, transaction, timeout);
            }
            var updateSql = string.Empty;
            var updateSqls = new List<string>();

            var classMap = ClassMapHelper.GetMap<T>();
            DynamicParameters parameters = new DynamicParameters();
            classMap.Each((column) =>
            {
                if (updateField.Contains(column.Name) || updateField.Contains(column.PropertyName))
                {
                    if (!column.CanUpdate)
                        throw DbException("UpdateFields", parameters, new Exception($"模型 {classMap.TableName} 中的属性 {column.Name} 配置为不能更新"));
                    updateSqls.Add($"{column.Name} = {sqlGenerator.ParameterPrefix}{column.Name}");
                }
                if (column.CanUpdate)
                {
                    object value = column.PropertyInfo.GetValue(entity, null);
                    parameters.Add($"{sqlGenerator.ParameterPrefix}{column.Name}", value);
                }
            });

            connection = NewConnection;

            try
            {
                updateSql = $"Update {classMap.TableName} SET {string.Join(",", updateSqls)} WHERE {classMap.TryGetIdWhere(sqlGenerator)};";
                return connection.Execute(updateSql, parameters, transaction, timeout ?? timeOutValue,
                    CommandType.Text) > 0;
            }
            catch (Exception ex) { throw DbException(updateSql, parameters, ex); }
            finally { SqlPrint?.Invoke(updateSql, JsonConvert.SerializeObject(parameters)); }
        }

        public T UpdateFields<T>(T entity, object updateobj, DbConnection connection = null, DbTransaction transaction = null, int? timeout = null) where T : class
        {
            var updateSql = string.Empty;
            var updateSqls = new List<string>();
            var isSuccess = false;
            var classMap = ClassMapHelper.GetMap<T>();
            var updateColumns = ReflectionHelper.GetDictionary(updateobj);
            DynamicParameters parameters = new DynamicParameters();

            using (var en = updateColumns.GetEnumerator())
            {
                while (en.MoveNext())
                {
                    var item = en.Current;

                    if (!classMap.TryGetColumn(item.Key, out DbColumnAttribute column)) continue;

                    if (column.IsKey || !column.CanUpdate) continue;

                    parameters.Add($"{sqlGenerator.ParameterPrefix}{column.Name}", item.Value);
                    updateSqls.Add(sqlGenerator.GetParameter(column));
                }
            }

            var keyValue = entity.GetEntityValue(classMap.Key.Name);
            parameters.Add($"{sqlGenerator.ParameterPrefix}{classMap.Key.Name}", keyValue);

            connection = NewConnection;
            try
            {
                updateSql = $"Update {classMap.TableName} SET {string.Join(",", updateSqls)} WHERE {classMap.TryGetIdWhere(sqlGenerator)};";
                isSuccess = connection.Execute(updateSql, parameters, transaction, timeout ?? timeOutValue,
                    CommandType.Text) > 0;

                if (isSuccess)
                {
                    foreach (var item in updateColumns)
                    {
                        entity.SetEntityValue(item.Key, item.Value);
                    }
                }
            }
            catch (Exception ex) { throw DbException(updateSql, parameters, ex); }
            finally { SqlPrint?.Invoke(updateSql, JsonConvert.SerializeObject(parameters)); }
            return entity;
        }
    }
}