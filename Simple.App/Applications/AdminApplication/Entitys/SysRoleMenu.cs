namespace Simple.AdminApplication.Entitys
{
    /// <summary>角色菜单</summary>
    public class SysRoleMenu : DefaultEntityInt
    {
        public int SysRoleId { get; set; }
        public int SysMenuId { get; set; }
        public virtual SysMenu SysMenu { get; set; }
        public virtual SysRole SysRole { get; set; }
    }
}