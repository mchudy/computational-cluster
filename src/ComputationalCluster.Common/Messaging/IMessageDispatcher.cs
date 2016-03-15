using ComputationalCluster.Common.Messages;
using System.Net.Sockets;

namespace ComputationalCluster.Common.Messaging
{
    public interface IMessageDispatcher
    {
        void Dispatch<T>(T message, NetworkStream stream)
            where T : Message;
    }
}