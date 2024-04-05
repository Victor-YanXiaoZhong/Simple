namespace Simple.AdminApplication.Entitys
{
    public class SysOrgnization : DefaultEntityInt
    {
        /// <summary>名称</summary>
        public string Name { get; set; }

        /// <summary>联系人</summary>
        public string? Contract { get; set; }

        /// <summary>是否是管理平台 默认false</summary>
        public bool IsAdmin { get; set; } = false;

        /// <summary>联系电话</summary>
        public string? Phone { get; set; }

        /// <summary>地址</summary>
        public string? Address { get; set; }

        /// <summary>电子邮箱</summary>
        public string? Email { get; set; }

        /// <summary>备注</summary>
        public string? Remark { get; set; }
    }
}