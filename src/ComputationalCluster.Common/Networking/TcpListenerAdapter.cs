using System.Net;
using System.Net.Sockets;

namespace ComputationalCluster.Common.Networking
{
    public class TcpListenerAdapter : ITcpListener
    {
        private readonly TcpListener listener;

        public TcpListenerAdapter(TcpListener listener)
        {
            this.listener = listener;
        }

        public void Start()
        {
            listener.Start();
        }

        public ITcpClient AcceptTcpClient()
        {
            return new TcpClientAdapter(listener.AcceptTcpClient());
        }

        public IPEndPoint LocalEndpoint => listener.LocalEndpoint as IPEndPoint;
    }
}
