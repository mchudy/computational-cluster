using System.Collections.Generic;
using ComputationalCluster.Common.Messages;

namespace ComputationalCluster.Common.Networking
{
    public interface IMessageStreamReader
    {
        Message ReadMessage();
        IList<Message> ReadToEnd();
    }
}