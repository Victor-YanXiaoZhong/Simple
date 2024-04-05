using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.AdminApplication.Entitys
{
    /// <summary>
    /// 角色权限
    /// </summary>
    public class SysRoleFunction : DefaultEntityInt
    {
        public int SysRoleId { get; set; }
        public int SysFunctionId { get; set; }
        public virtual SysFunction SysFunction { get; set; }
        public virtual SysRole SysRole { get; set; }
    }
}
