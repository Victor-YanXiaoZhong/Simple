namespace Simple.AdminApplication.Entitys
{
    public class SysUser : DefaultEntityInt
    {
        /// <summary>组织机构Id</summary>
        public int SysOrgnizationId { get; set; } = 0;

        /// <summary>账号</summary>
        public string Account { get; set; }

        /// <summary>密码</summary>
        public string Password { get; set; }

        /// <summary>密码加密因子</summary>
        public string? Seed { get; set; } = string.Empty;

        /// <summary>名称</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>头像</summary>
        public string? Avatar { get; set; }

        /// <summary>联系电话</summary>
        public string? Phone { get; set; }

        /// <summary>电子邮箱</summary>
        public string? Email { get; set; }

        /// <summary>是否是超级管理员</summary>
        public bool SupperAdmin { get; set; } = false;

        /// <summary>账户角色</summary>
        public int? SysRoleId { get; set; }

        /// <summary>账户角色</summary>
        public virtual SysRole? SysRole { get; set; }

        /// <summary>账户机构</summary>
        public virtual SysOrgnization? SysOrgnization { get; set; }
    }
}