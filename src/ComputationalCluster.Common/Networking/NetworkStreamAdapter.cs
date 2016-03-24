using System.Net.Sockets;

namespace ComputationalCluster.Common.Networking
{
    public class NetworkStreamAdapter : INetworkStream
    {
        private readonly NetworkStream stream;

        public NetworkStreamAdapter(TcpClient client)
        {
            stream = client.GetStream();
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            stream.Write(buffer, offset, count);
            stream.Flush();
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            return stream.Read(buffer, offset, count);
        }

        public bool DataAvailable => stream.DataAvailable;

        public void Dispose()
        {
            stream.Close();
        }
    }
}
