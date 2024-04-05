namespace Simple.Dapper
{
    public partial class DbComponent
    {
        public T FirstOrDefault<T>(object param = null, DbTransaction transaction = null, CommandType commandType = CommandType.Text)
        {
            DbConnection conn = null;
            T model = default;
            bool isSuccess = false;
            var classMap = ClassMapHelper.GetMap<T>();
            var commandText = sqlGenerator.Select(classMap);
            try
            {
                conn = NewConnection;
                model = conn.QueryFirstOrDefault<T>(commandText, param, transaction, timeOutValue, commandType);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                throw DbException(commandText, param, ex);
            }
            finally
            {
                conn.TryDispose();
            }
            return model;
        }
    }
}