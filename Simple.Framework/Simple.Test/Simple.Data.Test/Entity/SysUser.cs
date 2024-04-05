namespace Simple.Dapper.Test
{
    [DbTable("Sys_User")]
    public class SysUser
    {
        [DbColumn(canInsert: true, isIdentityKey: true)]
        public int Id { get; set; }

        public string Name { get; set; }

        [DbColumn(canUpdate: false)]
        public string Dec { get; set; }

        [DbColumn("State_Code")]
        public string StateCode { get; set; }

        public string UpdateTime { get; set; }
    }
}