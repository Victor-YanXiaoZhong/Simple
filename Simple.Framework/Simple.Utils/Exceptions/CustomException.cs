namespace Simple.Utils
{
    /// <summary>异常级别</summary>
    public enum ExceptionLevel
    {
        /// <summary>信息</summary>
        Info,

        /// <summary>警告</summary>
        Warn,

        /// <summary>错误</summary>
        Error,

        /// <summary>严重错误</summary>
        Fatal,
    }

    /// <summary>自定义异常</summary>
    public class CustomException : Exception
    {
        /// <summary>实例化</summary>
        /// <param name="message">异常消息</param>
        /// <param name="friendlyMessage">友好提示消息，可以直接展示给用户</param>
        public CustomException(string message, string friendlyMessage)
            : base(message)
        {
            FriendlyMessage = friendlyMessage;
        }

        /// <summary>实例化</summary>
        /// <param name="message">异常消息</param>
        /// <param name="friendlyMessage">友好提示消息，可以直接展示给用户</param>
        public CustomException(string message)
            : base(message)
        {
            FriendlyMessage = message;
        }

        /// <summary>实例化</summary>
        /// <param name="message">异常消息</param>
        /// <param name="friendlyMessage">友好提示消息，可以直接展示给用户</param>
        /// <param name="innerException">内部异常</param>
        public CustomException(string message, string friendlyMessage, Exception innerException)
            : base(string.Concat(message, innerException == null ? null : "消息：", innerException?.Message),
                   innerException)
        {
            FriendlyMessage = friendlyMessage;
        }

        /// <summary>实例化</summary>
        /// <param name="message">异常消息</param>
        /// <param name="friendlyMessage">友好提示消息，可以直接展示给用户</param>
        /// <param name="innerException">内部异常</param>
        public CustomException(string message, Exception innerException)
            : base(string.Concat(message, innerException == null ? null : "消息：", innerException?.Message),
                   innerException)
        {
            FriendlyMessage = message;
        }

        /// <summary>错误码</summary>
        public string ErrorCode { get; set; }

        /// <summary>结果码</summary>
        public string ResultCode { get; set; } = "500";

        /// <summary>友好提示消息（该值应满足安全呈现给用户的条件）</summary>
        public string FriendlyMessage { get; private set; }

        /// <summary>异常级别</summary>
        public ExceptionLevel Level { get; protected set; }
    }
}