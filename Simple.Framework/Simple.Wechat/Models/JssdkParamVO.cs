using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Wechat.Models
{
    public class JssdkParamVO
    {
        public string appId { get; set; }
        public string timestamp { get; set; }
        public string nonceStr { get; set; }
        public string signature { get; set; }
    }
}