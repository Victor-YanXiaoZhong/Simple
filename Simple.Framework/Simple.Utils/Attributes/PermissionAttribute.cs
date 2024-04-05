namespace Simple.Utils
{
    /// <summary>控制器Controller权限功能组标识</summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PermissionGroupAttribute : Attribute
    {
        public PermissionGroupAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>功能组名称</summary>
        public string Name { get; set; }
    }

    /// <summary>控制器Action权限标识 最终标识为 SYSUSER.EDIT(管理员-编辑)、 SYSUSER.DEL (管理员-删除)</summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class PermissionAttribute : PermissionGroupAttribute
    {
        public PermissionAttribute(string name, string dec, string group = "") : base(group)
        {
            this.Name = name;
            this.Description = dec;
            this.Group = group;
        }

        /// <summary>权限组</summary>
        public string Group { get; set; }

        /// <summary>权限名称</summary>
        public string Name { get; set; }

        /// <summary>描述</summary>
        public string Description { get; set; }
    }
}