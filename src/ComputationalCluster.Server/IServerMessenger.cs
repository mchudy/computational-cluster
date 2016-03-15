using System.Collections.Generic;
using System.Net.Sockets;
using ComputationalCluster.Common.Messages;

namespace ComputationalCluster.Server
{
    public interface IServerMessenger
    {
        void SendMessage(Message message, NetworkStream stream);
        void SendMessages(IList<Message> messages, NetworkStream stream);
    }
}