using ComputationalCluster.Common.Messages;

namespace ComputationalCluster.TaskManager
{
    public interface IResponseHandler<in T> where T : Message
    {
        void HandleResponse(T message);
    }
}
