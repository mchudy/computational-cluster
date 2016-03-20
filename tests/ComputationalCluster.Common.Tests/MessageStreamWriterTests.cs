using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Networking;
using ComputationalCluster.Common.Serialization;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace ComputationalCluster.Common.Tests
{
    public class MessageStreamWriterTests : IDisposable
    {
        private readonly NetworkStreamMock mockStream;
        private readonly Mock<IMessageSerializer> serializer;
        private readonly Mock<Message> message;

        public MessageStreamWriterTests()
        {
            mockStream = new NetworkStreamMock();
            serializer = new Mock<IMessageSerializer>();
            message = new Mock<Message>();
        }

        [Fact]
        public void WriteMessage_ShouldCallWriteOnNetworkStream()
        {
            var stream = new Mock<INetworkStream>();
            var writer = new MessageStreamWriter(stream.Object, serializer.Object);
            writer.WriteMessage(message.Object);

            stream.Verify(s => s.Write(It.IsAny<byte[]>(), 0, It.IsAny<int>()), Times.Once());
        }

        [Fact]
        public void WriteMessage_ShouldEndMessageWithSeparator()
        {
            var writer = new MessageStreamWriter(mockStream, serializer.Object);
            writer.WriteMessage(message.Object);

            byte[] result = mockStream.ToArray();
            Assert.Equal(1, result.Length);
            Assert.True(result[0] == Constants.ETB);
        }

        [Fact]
        public void WriteMessage_ShouldCorrectlyWriteMessage()
        {
            serializer.Setup(s => s.Serialize(It.IsAny<Message>())).Returns("aaa");
            var writer = new MessageStreamWriter(mockStream, serializer.Object);

            writer.WriteMessage(message.Object);

            byte[] result = mockStream.ToArray();
            byte[] expected = Encoding.UTF8.GetBytes("aaa" + Constants.ETB);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WriteMessages_ShouldSeparateMessages()
        {
            WriteTwoMessages();
            byte[] result = mockStream.ToArray();
            Assert.Equal(result.Count(b => b == Convert.ToByte(Constants.ETB)), 2);
        }

        [Fact]
        public void WriteMessages_ShouldCorrectlyWriteMessages()
        {
            WriteTwoMessages();
            byte[] result = mockStream.ToArray();
            byte[] expected = Encoding.UTF8.GetBytes("aaa" + Constants.ETB + "bbb" + Constants.ETB);
            Assert.Equal(expected, result);
        }

        private void WriteTwoMessages()
        {
            var message1 = new Mock<Message>();
            var message2 = new Mock<Message>();
            var writer = new MessageStreamWriter(mockStream, serializer.Object);
            serializer.Setup(s => s.Serialize(message1.Object)).Returns("aaa");
            serializer.Setup(s => s.Serialize(message2.Object)).Returns("bbb");

            writer.WriteMessages(new List<Message> { message1.Object, message2.Object });
        }

        public void Dispose()
        {
            mockStream.Close();
        }
    }
}
