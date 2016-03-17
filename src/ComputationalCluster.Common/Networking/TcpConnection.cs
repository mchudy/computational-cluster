using System;
using System.IO;
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

        public IPEndPoint EndPoint => client.Client.RemoteEndPoint as IPEndPoint;

        public void Connect(string address, int port)
        {
            client.Connect(address, port);
        }

        public Stream GetStream()
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
