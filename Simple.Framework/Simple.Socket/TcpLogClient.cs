using Simple.Utils;
using Simple.Utils.Helper;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Simple.Socket
{
    public class TcpLogClient
    {
        private static readonly string clientName = "LogClientDefault";
        private static readonly TcpClient tcpClient;
        private static readonly IPEndPoint logServerConfig;
        private static readonly ConcurrentStack<string> logStacks = new ConcurrentStack<string>();
        private static NetworkStream logStream;
        private static bool hasConnect = false;

        static TcpLogClient()
        {
            tcpClient = new TcpClient();
            clientName = ConfigHelper.GetValue("SocketLogServer:ClientName");
            var ip = ConfigHelper.GetValue("SocketLogServer:Ip");
            var port = ConfigHelper.GetValue<int>("SocketLogServer:Port");
            logServerConfig = new IPEndPoint(IPAddress.Parse(ip), port);

            ConnectLogServer();
        }

        private static void ConnectLogServer()
        {
            try
            {
                tcpClient.Connect(logServerConfig);
                tcpClient.Client.Send(GetMessage(clientName));
                logStream = tcpClient.GetStream();
                Thread.Sleep(200);
                hasConnect = true;
                Task.Run(SendLogTask);
            }
            catch (Exception)
            {
                hasConnect = false;
            }
        }

        private static void SendLogTask()
        {
            while (true)
            {
                try
                {
                    if (logStacks.IsEmpty || !hasConnect)
                    {
                        Thread.Sleep(300);
                        continue;
                    }
                    var issuccess = logStacks.TryPop(out string message);

                    if (!issuccess)
                        continue;

                    var msgBytes = GetMessage(message);
                    logStream.Write(msgBytes, 0, msgBytes.Length);
                    Thread.Sleep(300);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex.Message, ex);
                }
            }
        }

        private static byte[] GetMessage(string message)
        {
            return Encoding.UTF8.GetBytes($"{message}");
        }

        /// <summary>添加日志到发送堆栈中</summary>
        /// <param name="message"></param>
        public static void SendLog(string message)
        {
            logStacks.Push($"{DateTime.Now}: {message} \r\n");
        }
    }
}