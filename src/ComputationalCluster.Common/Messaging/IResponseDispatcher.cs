using ComputationalCluster.Common.Messages;

namespace ComputationalCluster.Common.Messaging
{
    public interface IResponseDispatcher
    {
        void Dispatch<T>(T message) where T : Message;
    }
}