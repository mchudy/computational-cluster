using System.IO;
using ComputationalCluster.Common.Networking;

namespace ComputationalCluster.Common.Tests
{
    public class NetworkStreamMock : MemoryStream, INetworkStream
    {
        public bool DataAvailable => Position < Length;
    }
}