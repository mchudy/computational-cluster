using System.Net.Sockets;
using ComputationalCluster.Common.Networking;

namespace ComputationalCluster.Common
{
    public class TcpConnectionFactory : ITcpConnectionFactory
    {
        public ITcpConnection Create()
        {
            return new TcpConnection(new TcpClient());
        }
    }
}
