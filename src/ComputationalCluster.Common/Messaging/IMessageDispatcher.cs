using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Networking;

namespace ComputationalCluster.Common.Messaging
{
    public interface IMessageDispatcher
    {
        void Dispatch<T>(T message, ITcpConnection connection)
            where T : Message;
    }
}