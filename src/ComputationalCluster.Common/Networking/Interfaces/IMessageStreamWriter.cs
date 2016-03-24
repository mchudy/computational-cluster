using System.Collections.Generic;
using ComputationalCluster.Common.Messages;

namespace ComputationalCluster.Common.Networking
{
    public interface IMessageStreamWriter
    {
        void WriteMessage(Message message);
        void WriteMessages(IList<Message> messages);
    }
}