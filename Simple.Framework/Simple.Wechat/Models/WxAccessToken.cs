using SKIT.FlurlHttpClient.Wechat.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Wechat.Models
{
    public class WxAccessToken : CgibinTokenResponse
    {
        public DateTime TokenExpireTime { get; set; }

        /// <summary>已过期（提前30分钟）</summary>
        public bool IsExpire
        {
            get
            {
                return DateTime.Now.AddMinutes(-5) > TokenExpireTime;
            }
        }
    }
}