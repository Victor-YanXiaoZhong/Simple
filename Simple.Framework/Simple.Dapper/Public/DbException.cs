namespace Simple.Dapper
{
    public class DbFatalException : FatalException
    {
        public DbFatalException(string message, string friendlyMessage = "", Exception ex = null) : base(message, friendlyMessage, ex)
        {
        }
    }
}