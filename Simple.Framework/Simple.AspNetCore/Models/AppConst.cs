using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.AspNetCore
{
    /// <summary>App 常量</summary>
    public class AppConst
    {
        static AppConst()
        {
            GloableErrResponse = @"{code:500,message:'服务器发生未处理的异常',success:false}";
        }

        public static string GloableErrResponse { get; set; }
    }
}