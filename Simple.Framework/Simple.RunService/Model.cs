using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.RunService
{
    /// <summary>配置文件</summary>
    public class service
    {
        /// <summary>服务唯一Id</summary>
        public string id { get; } = Guid.NewGuid().ToString();

        /// <summary>服务名称</summary>
        public string name { get; set; }

        /// <summary>服务描述</summary>
        public string description { get; set; }

        /// <summary>
        /// 参数
        ///eg： -Xrs -Xmx256m -jar
        /// </summary>
        public string arguments { get; set; }

        /// <summary>执行的程序 eg:myapp.exe</summary>
        public string executable { get; set; }
    }
}