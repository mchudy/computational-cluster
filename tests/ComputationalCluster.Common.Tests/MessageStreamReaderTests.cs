using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Networking;
using ComputationalCluster.Common.Serialization;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Xunit;

namespace ComputationalCluster.Common.Tests
{
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public class MessageStreamReaderTests : IDisposable
    {
        private readonly NetworkStreamMock networkStream;
        private readonly Mock<IMessageSerializer> serializer;
        private readonly Mock<Message> message;
        private readonly MessageStreamReader reader;

        public MessageStreamReaderTests()
        {
            networkStream = new NetworkStreamMock();
            serializer = new Mock<IMessageSerializer>();
            message = new Mock<Message>();
            serializer.Setup(s => s.Deserialize(It.IsAny<string>()))
                      .Returns<string>(s => new TextMessage { Text = s });
            reader = new MessageStreamReader(networkStream, serializer.Object);
        }

        [Fact]
        public void ReadMessage_ShouldCallReadOnNetworkStream()
        {
            var mockStream = new Mock<INetworkStream>();
            var streamReader = new MessageStreamReader(mockStream.Object, serializer.Object);

            streamReader.ReadMessage();

            mockStream.Verify(s => s.Read(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()), Times.AtLeastOnce());
        }

        [Fact]
        public void ReadToEnd_ShouldCallReadOnNetworkStream()
        {
            var mockStream = new Mock<INetworkStream>();
            var streamReader = new MessageStreamReader(mockStream.Object, serializer.Object);

            streamReader.ReadToEnd();

            mockStream.Verify(s => s.Read(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()), Times.AtLeastOnce());
        }

        [Fact]
        public void ReadMessage_ShouldReadUntilETB()
        {
            WriteBytes($"abc{Constants.ETB}");

            var response = reader.ReadMessage() as TextMessage;

            Assert.Equal("abc", response.Text);
        }

        [Fact]
        public void ReadMessage_ShouldReadUntilEndOfStream()
        {
            WriteBytesAndClose("abc");

            var response = reader.ReadMessage() as TextMessage;

            Assert.Equal("abc", response.Text);
        }

        [Fact]
        public void ReadToEnd_ShouldReadUntilEndOfStream()
        {
            WriteBytesAndClose("abc");

            var response = reader.ReadToEnd();

            Assert.Equal(1, response.Count);
            CheckMessages(response, "abc");
        }

        [Fact]
        public void ReadToEnd_ShouldReturnAllMessages()
        {
            WriteBytes($"abc{Constants.ETB}def");
            networkStream.Close();

            var response = reader.ReadToEnd();

            Assert.Equal(2, response.Count);
            CheckMessages(response, "abc", "def");
        }

        [Fact]
        public void ReadToEnd_WhenAllMessagesFinishedByETB_ShouldReturnAllMessages()
        {
            WriteBytesAndClose($"abc{Constants.ETB}def{Constants.ETB}");

            var response = reader.ReadToEnd();

            Assert.Equal(2, response.Count);
            CheckMessages(response, "abc", "def");
        }

        [Fact]
        public void ReadToEnd_ShouldIgnoreTrailingWhitespace()
        {
            WriteBytesAndClose($"abc{Constants.ETB}def{Constants.ETB}  \t\n");

            var response = reader.ReadToEnd();

            Assert.Equal(2, response.Count);
        }

        private void CheckMessages(IList<Message> messages, params string[] texts)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                Assert.Equal(texts[i], (messages[i] as TextMessage).Text);
            }
        }

        private void WriteBytes(string s)
        {
            var bytes = Encoding.UTF8.GetBytes(s);
            networkStream.Write(bytes, 0, bytes.Length);
            networkStream.Position = 0;
        }

        private void WriteBytesAndClose(string s)
        {
            WriteBytes(s);
            networkStream.Close();
        }

        public void Dispose()
        {
            networkStream.Close();
        }
    }
}
