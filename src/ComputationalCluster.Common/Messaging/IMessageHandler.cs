using ComputationalCluster.Common.Messages;
using System.Net.Sockets;

namespace ComputationalCluster.Common.Messaging
{
    public interface IMessageHandler<in T>
        where T : Message
    {
        //TODO: some server context instead of TcpClient
        void HandleMessage(T message, TcpClient client);
    }
}