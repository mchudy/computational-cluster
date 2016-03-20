using System;
using System.Net;

namespace ComputationalCluster.Common.Networking
{
    public interface ITcpClient : IDisposable
    {
        IPEndPoint EndPoint { get; }
        void Connect(string address, int port);
        INetworkStream GetStream();
    }
}