using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Simple.Socket
{
    public class UdpServer
    {
        /// <summary>客户端节点</summary>
        public ConcurrentDictionary<IPEndPoint, UdpClientState> clients = new ConcurrentDictionary<IPEndPoint, UdpClientState>();

        /// <summary>异步TCP服务器</summary>
        /// <param name="port"></param>
        public UdpServer(IPEndPoint localEP)
        {
            udpEP = localEP;
            Bind();
        }

        /// <summary>异步TCP服务器</summary>
        /// <param name="port"></param>
        public UdpServer(int port)
        {
            udpEP = new IPEndPoint(IPAddress.Any, port);
            Bind();
        }

        public event Action<string> MessageEv;

        public event Action<UdpClientState> UdpClientStateEv;

        ~UdpServer()
        {
            Stop();
        }

        public IPEndPoint udpEP { get; set; }

        #region 字段

        private bool isRuning = false;
        private UdpClient udpServer;

        #endregion 字段

        /// <summary>绑定端口</summary>
        private void Bind()
        {
            udpServer = new UdpClient(udpEP);
            udpServer.EnableBroadcast = true;
        }

        /// <summary>发送收到的消息</summary>
        /// <param name="clientName"></param>
        /// <param name="message"></param>
        private void OnUdpClientMessage(UdpClientState clientState)
        {
            UdpClientStateEv?.Invoke(clientState);
            MessageEv?.Invoke(clientState.Message);
        }

        /// <summary>发送收到的消息</summary>
        /// <param name="clientName"></param>
        /// <param name="message"></param>
        private void OnMessage(string message)
        {
            MessageEv?.Invoke($"{DateTime.Now}: {message}");
        }

        public void Start()
        {
            OnMessage($"监听服务已启动 port：{udpEP.Port}");
            isRuning = true;
            udpServer.BeginReceive(new AsyncCallback(ClientReciveHandAsync), udpServer);
        }

        public void Stop()
        {
            isRuning = false;
            OnMessage($"监听服务已停止 port：{udpEP.Port}");
            udpServer.Close();
            udpServer.Dispose();
        }

        /// <summary>异步处理收到的连接</summary>
        /// <param name="ar"></param>
        public void ClientReciveHandAsync(IAsyncResult ar)
        {
            if (!isRuning) return;

            try
            {
                if (!udpServer.Client.Connected) return;
                IPEndPoint clientEP = null;
                var data = udpServer.EndReceive(ar, ref clientEP);

                UdpClientState clientState = null;

                if (clients.ContainsKey(clientEP))
                {
                    clientState = clients[clientEP];
                    clientState.Buffer = data;
                }
                else
                {
                    var client = new UdpClient(clientEP);
                    clientState = new UdpClientState(client, data);
                    clients[clientEP] = clientState;
                }
                var dataStr = Encoding.UTF8.GetString(clientState.Buffer, 0, data.Length);
                clientState.Message = dataStr;
                OnUdpClientMessage(clientState);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                udpServer.BeginReceive(new AsyncCallback(ClientReciveHandAsync), udpServer);
            }
        }
    }
}