using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Simple.Socket
{
    /// <summary>异步TCP服务器</summary>
    public class TcpServer
    {
        private bool isRuning = false;
        public Func<byte[], string> ClientNameFunc = bt => Encoding.UTF8.GetString(bt);
        public ConcurrentDictionary<string, TcpClientState> ClientDic = new ConcurrentDictionary<string, TcpClientState>();

        /// <summary>异步TCP服务器</summary>
        /// <param name="port"></param>
        public TcpServer(IPEndPoint localEP)
            : this(localEP.Address, localEP.Port)
        {
        }

        /// <summary>异步TCP服务器</summary>
        /// <param name="port"></param>
        public TcpServer(IPAddress ipAddress, int port, Func<byte[], string> clientNameFunc = null)
        {
            TcpEP = new IPEndPoint(ipAddress, port);
            TcpListener = new TcpListener(TcpEP);
            TcpListener.AllowNatTraversal(true);
            if (clientNameFunc != null)
                ClientNameFunc = clientNameFunc;
        }

        public event Action<string, string> MessageEv;

        public event Action<TcpClientState> TcpClientStateEv;

        ~TcpServer()
        {
            Stop();
        }

        public TcpListener TcpListener { get; set; }
        public IPEndPoint TcpEP { get; set; }

        /// <summary>发送收到的消息</summary>
        /// <param name="clientName"></param>
        /// <param name="message"></param>
        private void OnTcpClientMessage(TcpClientState clientState)
        {
            TcpClientStateEv?.Invoke(clientState);
            MessageEv?.Invoke(clientState.Name, clientState.Message);
        }

        /// <summary>发送收到的消息</summary>
        /// <param name="clientName"></param>
        /// <param name="message"></param>
        private void OnMessage(string name, string message)
        {
            MessageEv?.Invoke(name, message);
        }

        /// <summary>启动监听服务器</summary>
        public void Start()
        {
            TcpListener.Start();
            isRuning = true;

            TcpListener.BeginAcceptTcpClient(new AsyncCallback(ClientConnectHandAsync), TcpListener);
            OnMessage("", $"端口 {TcpEP.Port}监听服务已启动");
        }

        /// <summary>停止监听fuwqu</summary>
        public void Stop()
        {
            foreach (var client in ClientDic)
            {
                client.Value.Close();
            }
            ClientDic.Clear();
            TcpListener.Stop();
            isRuning = false;
            OnMessage("", $"端口 {TcpEP.Port}监听服务已关闭");
        }

        /// <summary>异步处理收到的连接</summary>
        /// <param name="ar"></param>
        public void ClientConnectHandAsync(IAsyncResult ar)
        {
            if (!isRuning) return;

            try
            {
                var client = TcpListener.EndAcceptTcpClient(ar);
                var buffer = new byte[client.ReceiveBufferSize];
                var stream = client.GetStream();

                var clientState = new TcpClientState(client, buffer);

                var bytesRead = stream.Read(clientState.Buffer, 0, client.Available);
                var name = ClientNameFunc?.Invoke(clientState.Buffer.Take(bytesRead).ToArray());
                clientState.Name = name;
                clientState.Message = "新客户端接入";

                ClientDic[clientState.Name] = clientState;
                OnTcpClientMessage(clientState);
                stream.BeginRead(clientState.Buffer, 0, clientState.Buffer.Length, ClientDataHandAsync, clientState);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                TcpListener.BeginAcceptTcpClient(new AsyncCallback(ClientConnectHandAsync), TcpListener);
            }
        }

        /// <summary>异步处理收到的数据</summary>
        /// <param name="ar"></param>
        public void ClientDataHandAsync(IAsyncResult ar)
        {
            var state = (TcpClientState)ar.AsyncState;
            if (!state.TcpClient.Connected)
            {
                OnMessage(state.Name, "客户端已端开");
                ClientDic.TryRemove(state.Name, out TcpClientState outState);
                return;
            }

            if (!isRuning) return;

            var stream = state.TcpClient.GetStream();
            int recv = 0;

            try
            {
                try
                {
                    recv = state.TcpClient.Available;
                }
                catch (Exception)
                {
                    recv = 0;
                }

                if (recv == 0)
                {
                    ClientDic.TryRemove(state.Name, out var outState);
                }

                var data = new byte[recv];
                Buffer.BlockCopy(state.Buffer, 0, data, 0, recv);

                state.Message = Encoding.UTF8.GetString(data);
                OnTcpClientMessage(state);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                stream.BeginRead(state.Buffer, 0, recv, ClientDataHandAsync, state);
            }
        }

        public void SendToClient(TcpClientState state, byte[] data)
        {
            if (!state.TcpClient.Connected)
            {
                OnMessage(state.Name, "客户端不在线");
                return;
            }
        }

        /// <summary>异步发送数据到客户端</summary>
        /// <param name="client"></param>
        /// <param name="data"></param>
        public void SendToClient(TcpClient client, byte[] data)
        {
            if (!client.Connected)
            {
                return;
            }
            client.GetStream().BeginWrite(data, 0, data.Length, SendEnd, client);

            void SendEnd(IAsyncResult ar)
            {
                ((TcpClient)ar.AsyncState).GetStream().EndWrite(ar);
            }
        }
    }
}