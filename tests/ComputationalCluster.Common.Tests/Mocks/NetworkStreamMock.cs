using ComputationalCluster.Common.Networking;
using System.IO;

namespace ComputationalCluster.Common.Tests
{
    public class NetworkStreamMock : MemoryStream, INetworkStream
    {
        public bool DataAvailable => Position < Length;

        public override void Close()
        {

        }
    }
}