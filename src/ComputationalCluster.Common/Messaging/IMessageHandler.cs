using System.Net.Sockets;
using ComputationalCluster.Common.Messages;

namespace ComputationalCluster.Common.Messaging
{
    public interface IMessageHandler<in T>
        where T : Message
    {
        void HandleMessage(T message, TcpClient client);
    }
}