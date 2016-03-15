using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Utils;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ComputationalCluster.Common.Serialization
{
    public class MessageSerializer : IMessageSerializer
    {
        static MessageSerializer()
        {
            messageTypes = typeof(Message).Assembly
                .GetTypes()
                .Where(t => t.IsDefined(typeof(XmlRootAttribute), false))
                .ToArray();
        }

        private static readonly Type[] messageTypes;

        public bool Indent { get; set; } = true;

        public string Serialize(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException();
            }
            var serializer = new XmlSerializer(message.GetType());
            string messageString;
            using (var textWriter = new Utf8StringWriter())
            {
                var settings = new XmlWriterSettings { Indent = Indent, Encoding = Encoding.UTF8 };
                using (var xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, message);
                    messageString = textWriter.ToString();
                }
            }
            return messageString;
        }

        public Message Deserialize(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new ArgumentException();
            }
            var rootName = GetXmlRoot(xml);
            var rootType = GetRootType(rootName);
            var serializer = new XmlSerializer(rootType);
            Message result;
            using (var stream = new StringReader(xml))
            {
                result = (Message)serializer.Deserialize(stream);
            }
            return result;
        }

        private static Type GetRootType(string rootName)
        {
            var rootType = messageTypes.FirstOrDefault(t => t.GetCustomAttributes(typeof(XmlRootAttribute), true)
                .Cast<XmlRootAttribute>()
                .Any(a => a.ElementName == rootName));
            return rootType;
        }

        private string GetXmlRoot(string xml)
        {
            using (var stream = new StringReader(xml))
            {
                using (XmlReader xmlReader = XmlReader.Create(stream))
                {
                    if (xmlReader.MoveToContent() == XmlNodeType.Element)
                    {
                        return xmlReader.LocalName;
                    }
                }
            }
            return null;
        }
    }
}
