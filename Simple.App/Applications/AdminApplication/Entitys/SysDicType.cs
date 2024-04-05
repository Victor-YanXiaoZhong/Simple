using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.AdminApplication.Entitys
{
    /// <summary>字典类型</summary>
    public class SysDicType : DefaultEntityInt
    {
        public SysDicType()
        {
            SysDicValues = new HashSet<SysDicValue>();
        }

        /// <summary>名称</summary>
        public string Name { get; set; }

        /// <summary>备注</summary>
        public string? Remark { get; set; }

        public int Sort { get; set; } = 0;

        public virtual ICollection<SysDicValue> SysDicValues { get; set; }
    }
}