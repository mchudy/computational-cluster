using System;
using System.Net;
using System.Net.Sockets;

namespace ComputationalCluster.Common.Networking
{
    public class TcpConnection : ITcpConnection
    {
        private readonly TcpClient client;

        public TcpConnection(TcpClient client)
        {
            this.client = client;
        }

        public EndPoint EndPoint => client.Client.RemoteEndPoint;

        public void Connect(string address, int port)
        {
            client.Connect(address, port);
        }

        public NetworkStream GetStream()
        {
            return client.GetStream();
        }

        public void Dispose()
        {
            //client.GetStream().Close();
            client.Close();
            ((IDisposable)client).Dispose();
        }
    }
}
