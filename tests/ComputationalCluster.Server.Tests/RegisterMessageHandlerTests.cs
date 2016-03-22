using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Networking;
using ComputationalCluster.Server.Configuration;
using ComputationalCluster.Server.Handlers;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace ComputationalCluster.Server.Tests
{
    public class RegisterMessageHandlerTests
    {
        private readonly Mock<IServerMessenger> messenger = new Mock<IServerMessenger>();
        private readonly Mock<IStatusChecker> statusChecker = new Mock<IStatusChecker>();
        private readonly Mock<ITcpClient> tcpClient = new Mock<ITcpClient>();
        private readonly Mock<IServerConfiguration> config = new Mock<IServerConfiguration>();
        private readonly Mock<IServerContext> context = new Mock<IServerContext>();

        public RegisterMessageHandlerTests()
        {
            context.SetupGet(c => c.Configuration).Returns(config.Object);
            context.SetupGet(c => c.Nodes).Returns(new List<ComputationalNode>());
        }

        [Fact]
        public void ShouldSendResponseWithNewId()
        {
            var message = GetNodeMessage();
            context.Setup(c => c.GetNextComponentId()).Returns(5);
            var handler = new RegisterMessageHandler(messenger.Object, context.Object, statusChecker.Object);

            handler.HandleMessage(message, tcpClient.Object);

            messenger.Verify(m => m.SendMessage(It.Is<RegisterResponseMessage>(rm => rm.Id == 5), It.IsAny<INetworkStream>()));
        }

        [Fact]
        public void WhenNewNodeRegistering_ShouldAddNodeToContext()
        {
            var message = GetNodeMessage();
            var nodesList = new List<ComputationalNode>();
            context.SetupGet(c => c.Nodes).Returns(nodesList);
            context.Setup(c => c.GetNextComponentId()).Returns(5);
            var handler = new RegisterMessageHandler(messenger.Object, context.Object, statusChecker.Object);

            handler.HandleMessage(message, tcpClient.Object);

            Assert.Equal(1, nodesList.Count);
            Assert.Equal(5, nodesList[0].Id);
            Assert.Equal(message.ParallelThreads, nodesList[0].ThreadsCount);
            Assert.Equal(1, nodesList[0].SolvableProblems.Count);
            Assert.Equal("DVRP", nodesList[0].SolvableProblems[0]);
        }

        [Fact]
        public void ShouldSendBackTimeoutFromConfiguration()
        {
            var message = GetNodeMessage();
            var handler = new RegisterMessageHandler(messenger.Object, context.Object, statusChecker.Object);
            config.SetupGet(c => c.Timeout).Returns(10);
            handler.HandleMessage(message, tcpClient.Object);

            messenger.Verify(m => m.SendMessage(It.Is<RegisterResponseMessage>(rm => rm.Timeout == 10), It.IsAny<INetworkStream>()));
        }

        private static RegisterMessage GetNodeMessage()
        {
            var message = new RegisterMessage
            {
                Type = RegisterType.ComputationalNode,
                ParallelThreads = 4,
                SolvableProblems = new[] { "DVRP" }
            };
            return message;
        }
    }
}
