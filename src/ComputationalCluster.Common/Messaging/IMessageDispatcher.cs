using System.Net.Sockets;
using ComputationalCluster.Common.Messages;

namespace ComputationalCluster.Common.Messaging
{
    public interface IMessageDispatcher
    {
        void Dispatch<T>(T message, TcpClient client) where T : Message;
    }
}