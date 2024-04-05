using Microsoft.Extensions.Logging;
using Simple.Utils;

namespace Simple.EntityFrameworkCore
{
    internal class EFLoggerFactory : ILoggerFactory
    {
        public void AddProvider(ILoggerProvider provider)
        {
            throw new NotImplementedException();
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new EFLogger(categoryName);//创建EFLogger类的实例
        }

        public void Dispose()
        {
        }
    }

    public class EFLogger : ILogger
    {
        private readonly string categoryName;

        public EFLogger(string categoryName) => this.categoryName = categoryName;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            //ef core执行数据库查询时的categoryName为Microsoft.EntityFrameworkCore.Database.Command,日志级别为Information
            if (categoryName == "Microsoft.EntityFrameworkCore.Database.Command"
                && logLevel == LogLevel.Information)
            {
                var logContent = formatter(state, exception);
                LogHelper.Debug(logContent);

                this.Log(LogLevel.Trace, logContent);
            }
        }

        public IDisposable BeginScope<TState>(TState state) => null;
    }
}