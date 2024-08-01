namespace Simple.Dapper.Test
{
    [DbTable("Sys_User")]
    [DbJoinQuery("LEFT JOIN Sys_User_Role ON Sys_User.Id = Sys_User_Role.UserId")]
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

        [DbColumn(table: "Sys_User_Role")]
        public string RoleName { get; set; }
    }
}