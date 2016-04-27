using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Serialization;
using log4net;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace ComputationalCluster.Common.Networking
{
    public class MessageStreamReader : IMessageStreamReader
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(MessageStreamReader));

        public const int BufferSize = 1000000;

        private readonly INetworkStream stream;
        private readonly IMessageSerializer serializer;

        public MessageStreamReader(INetworkStream stream, IMessageSerializer serializer)
        {
            this.stream = stream;
            this.serializer = serializer;
        }

        public Message ReadMessage()
        {
            var xml = ReadToCharOrEndOfStream(Constants.ETB);
            logger.Debug(xml);
            return serializer.Deserialize(xml);
        }

        public IList<Message> ReadToEnd()
        {
            string responseString = ReadStreamToEnd();
            string[] messages = responseString.Split(Constants.ETB);
            var response = new List<Message>();
            foreach (var messageXml in messages)
            {
                if (string.IsNullOrWhiteSpace(messageXml))
                {
                    continue;
                }
                logger.Debug(messageXml);
                logger.Debug("----------------------------------------------------");
                var message = serializer.Deserialize(messageXml);
                response.Add(message);
            }
            return response;
        }

        private string ReadToCharOrEndOfStream(char delimiter)
        {
            byte[] buffer = new byte[BufferSize];
            using (var writer = new MemoryStream())
            {
                do
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead <= 0)
                    {
                        break;
                    }
                    if (buffer[bytesRead - 1] == delimiter)
                    {
                        writer.Write(buffer, 0, bytesRead - 1);
                        break;
                    }
                    writer.Write(buffer, 0, bytesRead);
                    Thread.Sleep(50);
                } while (stream.DataAvailable);
                return Encoding.UTF8.GetString(writer.ToArray());
            }
        }

        private string ReadStreamToEnd()
        {
            byte[] readBuffer = new byte[BufferSize];
            using (var writer = new MemoryStream())
            {
                do
                {
                    int bytesRead = stream.Read(readBuffer, 0, readBuffer.Length);
                    if (bytesRead <= 0)
                    {
                        break;
                    }
                    writer.Write(readBuffer, 0, bytesRead);
                    Thread.Sleep(50);
                } while (stream.DataAvailable);
                return Encoding.UTF8.GetString(writer.ToArray());
            }
        }
    }
}
