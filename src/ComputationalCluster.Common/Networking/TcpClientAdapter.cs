using System;
using System.Net;
using System.Net.Sockets;

namespace ComputationalCluster.Common.Networking
{
    public class TcpClientAdapter : ITcpClient
    {
        private readonly TcpClient client;

        public TcpClientAdapter(TcpClient client)
        {
            this.client = client;
        }

        public IPEndPoint EndPoint => client.Client.RemoteEndPoint as IPEndPoint;

        public void Connect(string address, int port)
        {
            client.Connect(address, port);
        }

        public INetworkStream GetStream()
        {
            return new NetworkStreamAdapter(client);
        }

        public void Dispose()
        {
            client.Close();
            ((IDisposable)client).Dispose();
        }
    }
}
