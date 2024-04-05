using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Socket
{
    /// <summary>
    /// 客户端载体
    /// </summary>
    public class UdpClientState
    {
        public UdpClientState(UdpClient client, byte[] buffer)
        {
            UdpClient = client;
            Buffer = buffer;

        }
        public string Name { get; set; }
        public UdpClient UdpClient { get; set; }
        public byte[] Buffer { get; set; }
        public string Message { get; set; }

        public void Close()
        {
            UdpClient?.Close();
            Buffer = null;
        }
    }
}
