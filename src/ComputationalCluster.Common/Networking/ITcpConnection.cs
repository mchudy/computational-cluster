using System;
using System.IO;
using System.Net;

namespace ComputationalCluster.Common.Networking
{
    public interface ITcpConnection : IDisposable
    {
        IPEndPoint EndPoint { get; }
        void Connect(string address, int port);
        Stream GetStream();
    }
}