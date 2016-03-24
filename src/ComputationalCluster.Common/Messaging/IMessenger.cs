using ComputationalCluster.Common.Messages;

namespace ComputationalCluster.Common.Messaging
{
    public interface IMessenger
    {
        void SendMessage(Message message);
    }
}