using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Serialization;
using System.Collections.Generic;
using System.Text;

namespace ComputationalCluster.Common.Networking
{
    public class MessageStreamWriter : IMessageStreamWriter
    {
        private readonly INetworkStream stream;
        private readonly IMessageSerializer serializer;

        public MessageStreamWriter(INetworkStream stream, IMessageSerializer serializer)
        {
            this.stream = stream;
            this.serializer = serializer;
        }

        public void WriteMessage(Message message)
        {
            string xml = serializer.Serialize(message);
            xml += Constants.ETB;
            byte[] bytes = Encoding.UTF8.GetBytes(xml);
            stream.Write(bytes, 0, bytes.Length);
        }

        public void WriteMessages(IList<Message> messages)
        {
            string xml = "";
            foreach (var message in messages)
            {
                xml += serializer.Serialize(message) + Constants.ETB;
            }
            byte[] bytes = Encoding.UTF8.GetBytes(xml);
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}
