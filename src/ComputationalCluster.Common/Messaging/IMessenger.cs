using System.Collections.Generic;
using ComputationalCluster.Common.Messages;

namespace ComputationalCluster.Common.Messaging
{
    public interface IMessenger
    {
        IList<Message> SendMessage(Message message);
        void SendMessageAndClose(Message message);
    }
}