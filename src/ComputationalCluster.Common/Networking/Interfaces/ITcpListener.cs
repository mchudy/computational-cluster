using System.Net;

namespace ComputationalCluster.Common.Networking
{
    public interface ITcpListener
    {
        void Start();
        ITcpClient AcceptTcpClient();
        IPEndPoint LocalEndpoint { get; }
    }
}
