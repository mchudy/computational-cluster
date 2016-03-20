using Xunit;

namespace ComputationalCluster.Client.Tests
{
    public class ClientTests
    {
        [Fact]
        public void SendMessage_ShouldConnectToServerAddressFromConfiguration()
        {
            //var memoryStream = new MemoryStream();
            //var configuration = new Mock<IConfiguration>();
            //configuration.Setup(c => c.ServerAddress).Returns("aaa");
            //configuration.Setup(c => c.ServerPort).Returns(1000);

            //var serializerMock = new Mock<IMessageSerializer>();
            //var tcpConnectionFactoryMock = new Mock<ITcpClientFactory>();
            //var tcpConnectionMock = new Mock<ITcpClient>();
            //tcpConnectionMock.Setup(t => t.GetStream()).Returns(memoryStream);
            //tcpConnectionFactoryMock.Setup(f => f.Create()).Returns(tcpConnectionMock.Object);

            //var messenger = new Messenger(serializerMock.Object, configuration.Object, tcpConnectionFactoryMock.Object);
            //messenger.SendMessage(new SolutionRequestMessage());

            //tcpConnectionMock.Verify(t => t.Connect("aaa", 1000), Times.Once());
        }
    }
}
