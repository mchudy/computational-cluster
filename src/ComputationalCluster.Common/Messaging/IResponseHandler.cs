using ComputationalCluster.Common.Messages;

namespace ComputationalCluster.Common.Messaging
{
    public interface IResponseHandler<in T> where T : Message
    {
        void HandleResponse(T message);
    }
}
