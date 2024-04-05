using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.AdminApplication.Entitys
{
    /// <summary>字典值</summary>
    public class SysDicValue : DefaultEntityInt
    {
        /// <summary>名称</summary>
        public string? Name { get; set; }

        public string Value { get; set; }

        /// <summary>备注</summary>
        public string? Remark { get; set; }

        public int SysDicTypeId { get; set; }

        public virtual SysDicType SysDicType { get; set; }
    }
}