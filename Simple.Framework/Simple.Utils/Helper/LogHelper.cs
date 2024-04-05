using NLog;

namespace Simple.Utils
{
    /// <summary>log封装类</summary>
    public class LogHelper
    {
        private static ILogger logger;

        static LogHelper()
        {
            var logConfigFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "nlog.config");
            if (!File.Exists(logConfigFile))
            {
                logConfigFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "nlog.config");
            }
            LogManager.LoadConfiguration(logConfigFile);
            logger = LogManager.GetCurrentClassLogger();
        }

        private static string FormateMessage(string message, Exception ex)
        {
            if (ex is null) return message;
            string formateMsg = $@"【抛出信息】:{message}
【异常类型】：{ex.GetType().Name}
【异常信息】:{ex.Message}
【内部信息】:{ex.InnerException?.Message}
【堆栈调用】:{ex.StackTrace}";
            return formateMsg;
        }

        /// <summary>输出错误日志到nlog</summary>
        /// <param name="t"></param>
        /// <param name="ex"></param>
        public static void Error(string message, Exception ex = null)
        {
            logger.Error(FormateMessage(message, ex));
        }

        /// <summary>记录消息日志</summary>
        /// <param name="t"></param>
        /// <param name="msg"></param>
        public static void Info(string message, Exception ex = null)
        {
            logger.Info(FormateMessage(message, ex));
        }

        /// 记录致命错误日志 </summary> <param name="t"></param> <param name="msg"></param>
        public static void Fatal(string message, Exception ex = null)
        {
            logger.Fatal(FormateMessage(message, ex));
        }

        /// <summary>记录Debug日志</summary>
        /// <param name="t"></param>
        /// <param name="msg"></param>
        public static void Debug(string message, object data = null)
        {
            logger.Debug(message + JsonHelper.ToJson(data));
        }

        /// <summary>记录警告信息</summary>
        /// <param name="t"></param>
        /// <param name="msg"></param>
        public static void Warn(string message, Exception ex = null)
        {
            logger.Warn(FormateMessage(message, ex));
        }
    }
}