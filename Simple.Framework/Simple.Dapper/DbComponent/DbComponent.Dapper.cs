namespace Simple.Dapper
{
    public partial class DbComponent
    {
        /// <summary>执行SQL 返回受影响的函数</summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        protected static int Excute(DbConnection connection, string commandtext, object param = null,
            DbTransaction transation = null, CommandType commandType = CommandType.Text)
        {
            try
            {
                return connection.Execute(commandtext, param, transation, timeOutValue, commandType);
            }
            catch (Exception ex)
            {
                throw DbException(commandtext, param, ex);
            }
        }

        protected static IDataReader ExecuteReader(DbConnection connection, string commandtext, object param = null,
                    DbTransaction transation = null, CommandType commandType = CommandType.Text)
        {
            try
            {
                return connection.ExecuteReader(commandtext, param, transation, timeOutValue, commandType);
            }
            catch (Exception ex)
            {
                throw DbException(commandtext, param, ex);
            }
        }

        protected static T ExecuteScalar<T>(DbConnection connection, string commandtext, object param = null,
                    DbTransaction transation = null, CommandType commandType = CommandType.Text)
        {
            try
            {
                return connection.ExecuteScalar<T>(commandtext, param, transation, timeOutValue, commandType);
            }
            catch (Exception ex)
            {
                throw DbException(commandtext, param, ex);
            }
        }

        /// <summary>执行SQL 自带事务</summary>
        /// <param name="commandText"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public bool Excute(string commandText, object param, CommandType commandType = CommandType.Text)
        {
            DbConnection conn = null;
            DbTransaction transaction = null;
            bool isSuccess = false;
            try
            {
                conn = NewConnection;
                transaction = conn.BeginTransaction();
                isSuccess = Excute(conn, commandText, param, transaction, commandType) > 0;
            }
            catch (Exception ex)
            {
                throw DbException(commandText, param, ex);
            }
            finally
            {
                Close(conn, transaction, isSuccess);
            }
            return isSuccess;
        }

        public IDataReader ExecuteReader(string commandText, object param, CommandType commandType = CommandType.Text)
        {
            DbConnection conn = null;
            DbTransaction transaction = null;
            IDataReader reader = null;
            bool isSuccess = false;
            try
            {
                conn = NewConnection;
                transaction = conn.BeginTransaction();
                reader = ExecuteReader(conn, commandText, param, transaction, commandType);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw DbException(commandText, param, ex);
            }
            finally
            {
                Close(conn, transaction, isSuccess);
            }
            return reader;
        }

        public T ExecuteScalar<T>(string commandText, object param, CommandType commandType = CommandType.Text)
        {
            DbConnection conn = null;
            DbTransaction transaction = null;
            T model = default;
            bool isSuccess = false;
            try
            {
                conn = NewConnection;
                transaction = conn.BeginTransaction();
                model = ExecuteScalar<T>(conn, commandText, param, transaction, commandType);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw DbException(commandText, param, ex);
            }
            finally
            {
                Close(conn, transaction, isSuccess);
            }
            return model;
        }

        public DataTable ExecuteDataTable(string commandText, object param = null, DbTransaction transaction = null, CommandType commandType = CommandType.Text)
        {
            DbConnection conn = null;
            IDataReader reader = null;
            DataTable table = new DataTable("TempTable");
            bool isSuccess = false;
            try
            {
                conn = NewConnection;
                reader = ExecuteReader(conn, commandText, param, transaction, commandType);
                table.Load(reader);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw DbException(commandText, param, ex);
            }
            finally
            {
                Close(conn, transaction, isSuccess);
            }
            return table;
        }

        /// <summary>执行存储过程，返回输出参数及值</summary>
        /// <param name="commandText"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public Dictionary<string, object> ExecuteProc(string commandText, object param = null, DbConnection conn = null, DbTransaction transaction = null)
        {
            bool isSuccess = false;
            var dict = new Dictionary<string, object>();
            try
            {
                if (conn == null)
                    conn = NewConnection;
                var dbParamDic = Utils.Helper.ReflectionHelper.GetDictionary(param);
                var dbParam = new List<DbParameter>();
                dbParamDic.Each(x =>
                {
                    dbParam.Add(CreateParameter(x.Key, x.Value));
                });

                var command = PrepareCommand(conn, transaction, commandText, dbParam, CommandType.StoredProcedure, timeOutValue);

                isSuccess = command.ExecuteNonQuery() > 0;

                foreach (IDbDataParameter parameter in command.Parameters)
                {
                    if (parameter.Direction != ParameterDirection.Input)
                        dict[parameter.ParameterName] = parameter.Value;
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw DbException(commandText, param, ex);
            }
            finally
            {
                Close(conn, transaction, isSuccess);
            }
            return dict;
        }
    }
}