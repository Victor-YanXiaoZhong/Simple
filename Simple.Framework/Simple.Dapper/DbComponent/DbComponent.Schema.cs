namespace Simple.Dapper
{
    public partial class DbComponent
    {
        /// <summary>获取数据库表信息</summary>
        /// <returns></returns>
        public List<DbTable> GetDbtables()
        {
            DbConnection? conn = null;
            var commandText = sqlGenerator.GetTableSchema();
            try
            {
                conn = NewConnection;
                return conn.Query<DbTable>(commandText).ToList();
            }
            catch (Exception ex)
            {
                throw DbException(commandText, ex: ex);
            }
            finally
            {
                conn?.TryDispose();
            }
        }

        /// <summary>获取数据库列信息</summary>
        /// <param name="tabName"></param>
        /// <returns></returns>
        public List<DbColumn> GetDbColumns(string tabName)
        {
            DbConnection? conn = null;
            var commandText = sqlGenerator.GetColumnSchema(tabName);
            try
            {
                conn = NewConnection;
                var result = conn.Query<DbColumn>(commandText).ToList();
                foreach (var item in result)
                {
                    var success = dbTypes.TryGetValue(item.DbType.ToLower(), out string? codeType);
                    item.CodeType = success ? codeType : "string";
                }
                return result;
            }
            catch (Exception ex)
            {
                throw DbException(commandText, ex: ex);
            }
            finally
            {
                conn?.TryDispose();
            }
        }
    }
}