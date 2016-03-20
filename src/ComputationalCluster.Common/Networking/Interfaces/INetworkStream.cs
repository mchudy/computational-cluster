using System;

namespace ComputationalCluster.Common.Networking
{
    public interface INetworkStream : IDisposable
    {
        void Write(byte[] buffer, int offset, int count);
        int Read(byte[] buffer, int offset, int count);
        bool DataAvailable { get; }
    }
}