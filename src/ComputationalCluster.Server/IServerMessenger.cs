using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Networking;
using System.Collections.Generic;

namespace ComputationalCluster.Server
{
    public interface IServerMessenger
    {
        void SendMessage(Message message, INetworkStream stream);
        void SendMessages(IList<Message> messages, INetworkStream stream);
        void SendToBackup(IList<Message> messages);
        void SendToBackup(Message message);
    }
}