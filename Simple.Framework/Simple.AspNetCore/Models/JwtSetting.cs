using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.AspNetCore.Models
{
    /// <summary>
    /// Jwt设置
    /// </summary>
    public class JwtSetting
    {

        /// <summary>
        /// 发行人
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 订阅者
        /// </summary>
        public string Audience { get; set; }


        /// <summary>
        /// 加密key
        /// </summary>
        public string SecurityKey { get; set; }


        /// <summary>
        /// 过期分钟
        /// </summary>
        public int ExpMinutes { get; set; }

    }
}