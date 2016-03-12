using ComputationalCluster.Common.Messages;

namespace ComputationalCluster.Common
{
    public interface IMessageHandler<in T>
        where T : Message
    {
        void HandleMessage(T message);
    }
}