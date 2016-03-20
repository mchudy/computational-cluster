using System.Collections.Generic;
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Networking;

namespace ComputationalCluster.Server
{
    public interface IServerMessenger
    {
        void SendMessage(Message message, INetworkStream stream);
        void SendMessages(IList<Message> messages, INetworkStream stream);
    }
}