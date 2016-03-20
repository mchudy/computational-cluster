using ComputationalCluster.Common.Serialization;

namespace ComputationalCluster.Common.Networking.Factories
{
    public class MessageStreamFactory : IMessageStreamFactory
    {
        private readonly IMessageSerializer serializer;

        public MessageStreamFactory(IMessageSerializer serializer)
        {
            this.serializer = serializer;
        }

        public IMessageStreamReader CreateReader(INetworkStream stream)
        {
            return new MessageStreamReader(stream, serializer);
        }

        public IMessageStreamWriter CreateWriter(INetworkStream stream)
        {
            return new MessageStreamWriter(stream, serializer);
        }
    }
}
