using System.Net.Sockets;

namespace Simple.Socket
{
    /// <summary>Tcp客户端连接参数</summary>
    public class TcpClientState
    {
        public TcpClientState(TcpClient client, byte[] buffer)
        {
            TcpClient = client;
            Buffer = buffer;
        }

        public string Name { get; set; }
        public TcpClient TcpClient { get; set; }
        public byte[] Buffer { get; private set; }
        public string Message { get; set; }

        public void Close()
        {
            TcpClient?.Close();
            Buffer = null;
        }
    }
}