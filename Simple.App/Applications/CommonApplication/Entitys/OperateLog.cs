using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.CommonApplication.Entitys
{
    public class OperateLog : DefaultEntityInt
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public string OperateType { get; set; }

        /// <summary>
        /// 操作描述
        /// </summary>
        public string Description { get; set; }
    }
}