namespace Simple.Dapper
{
    public class SQLServerGenerator : SQLGeneratorBase
    {
        public override string GetIdentity()
        {
            return "SELECT @@identity";
        }

        public override string GetVersion()
        {
            return "SELECT @@Version;";
        }
    }
}