using System.Net.Sockets;
using ComputationalCluster.Common.Networking;

namespace ComputationalCluster.Common
{
    public class TcpClientFactory : ITcpClientFactory
    {
        public ITcpClient Create()
        {
            return new TcpClientAdapter(new TcpClient());
        }
    }
}
