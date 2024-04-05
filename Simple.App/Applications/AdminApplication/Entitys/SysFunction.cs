namespace Simple.AdminApplication.Entitys
{
    /// <summary>系统功能点 系统启动后，自动生成</summary>
    public class SysFunction : DefaultEntityInt
    {
        public SysFunction()
        {
            SysMenus = new HashSet<SysMenu>();
            SysMenuFunctions = new HashSet<SysMenuFunction>();
        }

        /// <summary>功能点分组</summary>
        public string FunGroup { get; set; }

        /// <summary>功能点标识 controllername / actionname</summary>
        public string FunSign { get; set; }

        /// <summary>功能名称</summary>
        public string Name { get; set; }

        /// <summary>功能描述</summary>
        public string? Description { get; set; }

        public bool Enabled { get; set; } = true;

        public virtual ICollection<SysMenu> SysMenus { get; set; }
        public virtual ICollection<SysMenuFunction> SysMenuFunctions { get; set; }
    }
}