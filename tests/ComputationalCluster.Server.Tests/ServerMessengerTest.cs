using Xunit;

namespace ComputationalCluster.Server.Tests
{
    public class ServerMessengerTest
    {
        [Fact]
        public void SendMessage_ShouldFlushStream()
        {
            //var stream = new Mock<Stream>();
            //var serializer = new Mock<IMessageSerializer>();
            //serializer.Setup(s => s.Serialize(It.IsAny<Message>())).Returns("aa");
            //var messenger = new ServerMessenger(serializer.Object);
            //var messages = new List<Message>
            //    {
            //        new RegisterMessage
            //        {}
            //    };
            //messenger.SendMessages(messages, stream.Object);
            //stream.Verify(s => s.Flush(), Times.Once());
        }

        [Fact]
        public void SendMessages_ShouldFlushStream()
        {
            //var stream = new Mock<Stream>();
            //var serializer = new Mock<IMessageSerializer>();
            //serializer.Setup(s => s.Serialize(It.IsAny<Message>())).Returns("aa");
            //var messenger = new ServerMessenger(serializer.Object);
            //var message = new RegisterMessage();
            //messenger.SendMessage(message, stream.Object);
            //stream.Verify(s => s.Flush(), Times.Once());
        }
    }
}

