namespace Simple.AdminApplication.Entitys
{
    public class SysMenu : DefaultEntityInt
    {
        public SysMenu()
        {
            SysFunctions = new HashSet<SysFunction>();
            SysMenuFunctions = new HashSet<SysMenuFunction>();
            SysRoleMenus = new HashSet<SysRoleMenu>();
            SysRoles = new HashSet<SysRole>();
        }

        /// <summary>菜单名称</summary>
        public string Name { get; set; }

        /// <summary>菜单地址</summary>
        public string? Url { get; set; }

        /// <summary>菜单图标</summary>
        public string? Icon { get; set; }

        /// <summary>父级Id</summary>
        public int ParentId { get; set; } = 0;

        /// <summary>是否显示菜单</summary>
        public bool IsShow { get; set; } = true;

        /// <summary>是否可关闭</summary>
        public bool IsClose { get; set; } = true;

        public int Sort { get; set; } = 0;

        public virtual ICollection<SysFunction> SysFunctions { get; set; }
        public virtual ICollection<SysMenuFunction> SysMenuFunctions { get; set; }

        public virtual ICollection<SysRole> SysRoles { get; set; }
        public virtual ICollection<SysRoleMenu> SysRoleMenus { get; set; }
    }
}