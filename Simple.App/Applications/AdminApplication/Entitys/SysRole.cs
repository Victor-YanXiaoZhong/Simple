using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.AdminApplication.Entitys
{
    /// <summary>
    /// 角色
    /// </summary>
    public class SysRole : DefaultEntityInt
    {
        public SysRole()
        {
            SysMenus = new HashSet<SysMenu>();
            SysRoleMenus = new HashSet<SysRoleMenu>();
            SysUsers = new HashSet<SysUser>();
        }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string? Name { get; set; }

        public bool Enabled { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        public virtual ICollection<SysMenu> SysMenus { get; set; }
        public virtual ICollection<SysRoleMenu> SysRoleMenus { get; set; }

        public virtual ICollection<SysUser> SysUsers { get; set; }
    }
}
