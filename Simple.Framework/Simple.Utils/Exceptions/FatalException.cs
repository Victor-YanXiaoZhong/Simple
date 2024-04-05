using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Utils
{
    /// <summary>严重错误异常</summary>
    public class FatalException : CustomException
    {
        /// <summary>实例化</summary>
        /// <param name="message">异常消息</param>
        /// <param name="friendlyMessage">友好提示消息，可以直接展示给用户</param>
        public FatalException(string message, string friendlyMessage = "")
            : base(message, friendlyMessage)
        {
            Level = ExceptionLevel.Fatal;
        }

        /// <summary>实例化</summary>
        /// <param name="message">异常消息</param>
        /// <param name="innerException">内部异常</param>
        public FatalException(string message, Exception innerException)
            : base(message, "", innerException)
        {
            Level = ExceptionLevel.Fatal;
        }

        /// <summary>实例化</summary>
        /// <param name="message">异常消息</param>
        /// <param name="friendlyMessage">友好提示消息，可以直接展示给用户</param>
        /// <param name="innerException">内部异常</param>
        public FatalException(string message, string friendlyMessage, Exception innerException)
            : base(message, friendlyMessage, innerException)
        {
            Level = ExceptionLevel.Fatal;
        }
    }
}
