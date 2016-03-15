using ComputationalCluster.Common.Messages;
using System.Net.Sockets;

namespace ComputationalCluster.Common.Messaging
{
    public interface IMessageHandler<in T>
        where T : Message
    {
        void HandleMessage(T message, NetworkStream stream);
    }
}