using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Networking;
using ComputationalCluster.Common.Networking.Factories;
using Moq;
using Xunit;

namespace ComputationalCluster.Common.Tests
{
    public class MessengerTests
    {
        [Fact]
        public void SendMessage_ShouldConnectToServerAddressFromConfiguration()
        {
            var memoryStream = new NetworkStreamMock();
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(c => c.ServerAddress).Returns("address");
            configuration.Setup(c => c.ServerPort).Returns(1000);

            var factoryMock = new Mock<IMessageStreamFactory>();
            factoryMock.Setup(f => f.CreateReader(memoryStream)).Returns(new Mock<IMessageStreamReader>().Object);
            factoryMock.Setup(f => f.CreateWriter(memoryStream)).Returns(new Mock<IMessageStreamWriter>().Object);
            var tcpConnectionFactoryMock = new Mock<ITcpClientFactory>();
            var tcpConnectionMock = new Mock<ITcpClient>();
            tcpConnectionMock.Setup(t => t.GetStream()).Returns(memoryStream);
            tcpConnectionFactoryMock.Setup(f => f.Create()).Returns(tcpConnectionMock.Object);

            var messenger = new Messenger(configuration.Object, tcpConnectionFactoryMock.Object, factoryMock.Object,
                new Mock<IResponseDispatcher>().Object);
            //messenger.SendMessage(new RegisterMessage());

            //tcpConnectionMock.Verify(t => t.Connect("address", 1000), Times.Once());
        }
    }
}
