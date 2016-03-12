using ComputationalCluster.Common.Messages;
using System.Net.Sockets;

namespace ComputationalCluster.Common
{
    public interface IMessageDispatcher
    {
        void Dispatch<T>(T message, TcpClient client) where T : Message;
    }
}