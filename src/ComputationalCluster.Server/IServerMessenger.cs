using ComputationalCluster.Common.Messages;
using System.Collections.Generic;
using System.IO;

namespace ComputationalCluster.Server
{
    public interface IServerMessenger
    {
        void SendMessage(Message message, Stream stream);
        void SendMessages(IList<Message> messages, Stream stream);
    }
}