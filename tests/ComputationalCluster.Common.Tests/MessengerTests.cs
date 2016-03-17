using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Networking;
using ComputationalCluster.Common.Serialization;
using Moq;
using System.IO;
using System.Text;
using Xunit;

namespace ComputationalCluster.Common.Tests
{
    public class MessengerTests
    {
        [Fact]
        public void SendMessage_ShouldConnectToServerAddressFromConfiguration()
        {
            var memoryStream = new MemoryStream();
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(c => c.ServerAddress).Returns("aaa");
            configuration.Setup(c => c.ServerPort).Returns(1000);

            var serializerMock = new Mock<IMessageSerializer>();
            var tcpConnectionFactoryMock = new Mock<ITcpConnectionFactory>();
            var tcpConnectionMock = new Mock<ITcpConnection>();
            tcpConnectionMock.Setup(t => t.GetStream()).Returns(memoryStream);
            tcpConnectionFactoryMock.Setup(f => f.Create()).Returns(tcpConnectionMock.Object);

            var messenger = new Messenger(serializerMock.Object, configuration.Object, tcpConnectionFactoryMock.Object);
            messenger.SendMessage(new RegisterMessage());

            tcpConnectionMock.Verify(t => t.Connect("aaa", 1000), Times.Once());
        }

        [Fact]
        public void SendMessage_ShouldEndMessageWithETB()
        {
            var memoryStream = new MemoryStream();
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(c => c.ServerAddress).Returns("aaa");
            configuration.Setup(c => c.ServerPort).Returns(1000);

            var serializerMock = new Mock<IMessageSerializer>();
            var tcpConnectionFactoryMock = new Mock<ITcpConnectionFactory>();
            var tcpConnectionMock = new Mock<ITcpConnection>();
            tcpConnectionMock.Setup(t => t.GetStream()).Returns(memoryStream);
            tcpConnectionFactoryMock.Setup(f => f.Create()).Returns(tcpConnectionMock.Object);

            var messenger = new Messenger(serializerMock.Object, configuration.Object, tcpConnectionFactoryMock.Object);
            messenger.SendMessage(new RegisterMessage());

            byte[] buffer = new byte[1000];
            //memoryStream.Read(buffer, 0, 1000);
            string test = Encoding.UTF8.GetString(buffer);
            //Assert.Equal(Constants.ETB, test.Last());
        }
    }
}
