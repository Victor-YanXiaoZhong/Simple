using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Utils.Models.BO
{
    /// <summary>
    /// 权限模型
    /// </summary>
    public class PermissionBO
    {
        public string Group { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Sign { get; set; }
    }
}
