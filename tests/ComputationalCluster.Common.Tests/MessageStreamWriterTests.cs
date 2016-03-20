using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Networking;
using ComputationalCluster.Common.Serialization;
using Moq;
using System;
using Xunit;

namespace ComputationalCluster.Common.Tests
{
    public class MessageStreamWriterTests : IDisposable
    {
        private NetworkStreamMock mockStream;
        private Mock<IMessageSerializer> serializer;

        public MessageStreamWriterTests()
        {
            mockStream = new NetworkStreamMock();
            serializer = new Mock<IMessageSerializer>();
        }

        [Fact]
        public void Write_ShouldEndMessageWithSeparator()
        {
            var writer = new MessageStreamWriter(mockStream, serializer.Object);
            var message = new Mock<Message>();
            writer.WriteMessage(message.Object);

            byte[] result = new byte[4096];
            mockStream.Position = 0;
            mockStream.Read(result, 0, result.Length);
            Assert.True(result[0] == Constants.ETB);
        }


        public void Dispose()
        {
            mockStream.Close();
        }
    }
}
