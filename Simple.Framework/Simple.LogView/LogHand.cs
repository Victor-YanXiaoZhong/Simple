using Simple.Socket;
using System.Net;

namespace Simple.LogView
{
    internal class LogHand
    {
        public LogHand(int port = 10001)
        {
            this.TcpPort = port;
            TcpServer = new TcpServer(IPAddress.Any, port);
            TcpServer.MessageEv += TcpServer_MessageEv; ;
            TcpServer.Start();
        }

        public event Action<string, string> MessageEv;

        public TcpServer TcpServer { get; set; }
        public int TcpPort { get; set; }

        private void TcpServer_MessageEv(string name, string message)
        {
            MessageEv?.Invoke(name, message);
        }
    }
}