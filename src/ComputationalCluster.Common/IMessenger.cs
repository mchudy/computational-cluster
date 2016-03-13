using ComputationalCluster.Common.Messages;
using System.Collections.Generic;

namespace ComputationalCluster.Common
{
    public interface IMessenger
    {
        IList<Message> SendMessage(Message message);
        void SendMessageAndClose(Message message);
    }
}