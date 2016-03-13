using ComputationalCluster.Common.Messages;

namespace ComputationalCluster.Common.Serialization
{
    public interface IMessageSerializer
    {
        bool Indent { get; set; }

        Message Deserialize(string xml);
        string Serialize(Message message);
    }
}