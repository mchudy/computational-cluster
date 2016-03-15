using ComputationalCluster.Common;
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace ComputationalCluster.Server
{
    public class ServerMessenger : IServerMessenger
    {
        private readonly IMessageSerializer serializer;

        public ServerMessenger(IMessageSerializer serializer)
        {
            this.serializer = serializer;
        }

        public void SendMessages(IList<Message> messages, NetworkStream stream)
        {
            using (var writer = new StreamWriter(stream))
            {
                foreach (var message in messages)
                {
                    string xml = serializer.Serialize(message);
                    writer.Write(xml);
                    writer.Write(Constants.ETB);
                }
                writer.Flush();
            }
        }

        public void SendMessage(Message message, NetworkStream stream)
        {
            using (var writer = new StreamWriter(stream))
            {
                string xml = serializer.Serialize(message);
                writer.Write(xml);
                writer.Write(Constants.ETB);
                writer.Flush();
            }
        }
    }
}
