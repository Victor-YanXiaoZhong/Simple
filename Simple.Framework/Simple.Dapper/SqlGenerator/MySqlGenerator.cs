namespace Simple.Dapper
{
    public class MySqlGenerator : SQLGeneratorBase
    {
        public override string ColumnDot { get => "`"; }

        public override string GetIdentity()
        {
            return "SELECT @@IDENTITY";
        }
    }
}