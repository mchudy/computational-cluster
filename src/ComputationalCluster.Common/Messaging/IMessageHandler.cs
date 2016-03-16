using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Networking;

namespace ComputationalCluster.Common.Messaging
{
    public interface IMessageHandler<in T>
        where T : Message
    {
        void HandleMessage(T message, ITcpConnection connection);
    }
}