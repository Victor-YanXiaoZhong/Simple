using Simple.Utils.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Data.Test.Entity
{
    public class SysRole
    {
        public virtual int Id { get; set; } = default;

        /// <summary>是否已删除</summary>
        public bool Deleted { get; set; } = false;

        /// <summary>更新时间</summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>创建时间</summary>
        public virtual DateTime CreateTime { get; set; } = DateTime.Now;

        public string Name { get; set; }
        public string Remark { get; set; }
    }
}