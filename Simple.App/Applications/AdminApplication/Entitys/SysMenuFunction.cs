using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.AdminApplication.Entitys
{
    /// <summary>
    /// 菜单的功能
    /// </summary>
    public class SysMenuFunction : DefaultEntityInt
    {
        public int SysMenuId { get; set; }
        public virtual SysMenu SysMenu { get; set; }

        public int SysFunctionId { get; set; }
        public virtual SysFunction SysFunction { get; set; }
    }
}
