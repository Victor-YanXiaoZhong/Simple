using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.JobManageConsole.Help
{
    internal class LogHelper
    {
        private static readonly ConcurrentQueue<LogItem> logQueue = new ConcurrentQueue<LogItem>();
        private static bool isRunning = false;

        private static void ProcessLog()
        {
            isRunning = true;
            while (isRunning || !logQueue.IsEmpty)
            {
                if (logQueue.TryDequeue(out LogItem logItem))
                {
                    WriteToFile(logItem);
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
        }

        private static void WriteToFile(LogItem logItem)
        {
            var logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", logItem.SubPath);

            if (!Directory.Exists(logFilePath)) Directory.CreateDirectory(logFilePath);

            var logFile = Path.Combine(logFilePath, $"{logItem.LogType}_{DateTime.Now.ToString("yyyy-MM-dd")}.log");
            using (StreamWriter sw = new StreamWriter(logFile, true))
            {
                sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {logItem.Message}");
            }
        }

        public static void Waring(string nessage, string subPath = "", string logType = "Waring")
        {
            Log(new LogItem { Message = nessage, SubPath = subPath, LogType = logType });
        }

        public static void Info(string nessage, string subPath = "", string logType = "Info")
        {
            Log(new LogItem { Message = nessage, SubPath = subPath, LogType = logType });
        }

        public static void Err(string nessage, string subPath = "", string logType = "Err")
        {
            Log(new LogItem { Message = nessage, SubPath = subPath, LogType = logType });
        }

        public static void Log(LogItem item)
        {
            logQueue.Enqueue(item);
            if (!isRunning)
            {
                Task.Run(ProcessLog);
            }
        }
    }

    public class LogItem
    {
        /// <summary>上级目录</summary>
        public string SubPath { get; set; }

        /// <summary>类型 info waring err data</summary>
        public string LogType { get; set; } = "Info";

        public string Message { get; set; }
    }
}