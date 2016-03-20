using ComputationalCluster.Common.Messages;
using System.Collections.Generic;

namespace ComputationalCluster.Common.Messaging
{
    public interface IMessenger
    {
        IList<Message> SendMessage(Message message);
    }
}