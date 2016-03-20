using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Serialization;
using log4net;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ComputationalCluster.Common.Networking
{
    public class MessageStreamReader
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(MessageStreamReader));

        private readonly INetworkStream stream;
        private readonly IMessageSerializer serializer;

        public MessageStreamReader(INetworkStream stream, IMessageSerializer serializer)
        {
            this.stream = stream;
            this.serializer = serializer;
        }

        public Message ReadMessage()
        {
            var xml = ReadToChar(Constants.ETB);
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
                if (string.IsNullOrEmpty(messageXml))
                {
                    continue;
                }
                logger.Debug(messageXml);
                response.Add(serializer.Deserialize(messageXml));
            }
            return response;
        }

        //TODO: use MemoryStream in case the message is bigger than the buffer
        private string ReadToChar(char delimiter)
        {
            int bytesRead;
            int totalBytesRead = 0;
            byte[] buffer = new byte[Constants.BufferSize];
            while ((bytesRead =
                stream.Read(buffer, totalBytesRead, buffer.Length - totalBytesRead)) != 0)
            {
                totalBytesRead += bytesRead;
                if (buffer[totalBytesRead - 1] == delimiter)
                {
                    return Encoding.UTF8.GetString(buffer, 0, totalBytesRead - 1);
                }
            }
            return null;
        }

        private string ReadStreamToEnd()
        {
            byte[] readBuffer = new byte[Constants.BufferSize];
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
                } while (stream.DataAvailable);
                return Encoding.UTF8.GetString(writer.ToArray());
            }
        }
    }
}
