namespace Simple.Dapper
{
    public class SQLiteGenerator : SQLGeneratorBase
    {
        public override string GetIdentity()
        {
            return "SELECT LAST_INSERT_ROWID() AS ID";
        }

        public override string GetVersion()
        {
            return "SELECT version();";
        }
    }
}