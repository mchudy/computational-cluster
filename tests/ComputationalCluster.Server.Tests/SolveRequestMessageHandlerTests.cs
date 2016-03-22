using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Networking;
using ComputationalCluster.Server.Handlers;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace ComputationalCluster.Server.Tests
{
    public class SolveRequestMessageHandlerTests
    {
        private const int mockId = 5;
        private readonly Mock<IServerMessenger> messenger = new Mock<IServerMessenger>();
        private readonly Mock<IServerContext> context = new Mock<IServerContext>();
        private readonly Mock<ITcpClient> tcpClient = new Mock<ITcpClient>();
        private readonly SolveRequestMessage message;

        public SolveRequestMessageHandlerTests()
        {
            context.SetupGet(c => c.Problems).Returns(new List<ProblemInstance>());
            context.Setup(c => c.GetNextProblemId()).Returns(mockId);
            message = GetMessage();
        }

        [Fact]
        public void ShouldSendResponseWithNewId()
        {
            var handler = new SolveRequestMessageHandler(messenger.Object, context.Object);
            handler.HandleMessage(message, tcpClient.Object);

            messenger.Verify(m => m.SendMessage(It.Is<SolveRequestResponseMessage>(msg => msg.Id == 5), It.IsAny<INetworkStream>()));
        }

        [Fact]
        public void ShouldAddProblemToContext()
        {
            var problems = new List<ProblemInstance>();
            context.SetupGet(c => c.Problems).Returns(problems);
            var handler = new SolveRequestMessageHandler(messenger.Object, context.Object);

            handler.HandleMessage(message, tcpClient.Object);

            Assert.Equal(1, problems.Count);
            Assert.Equal("DVRP", problems[0].ProblemType);
            Assert.Equal(5, problems[0].Id);
            Assert.Equal(1000u, problems[0].SolvingTimeout);
        }

        [Fact]
        public void ShouldSetProblemStatusToNew()
        {
            var problems = new List<ProblemInstance>();
            context.SetupGet(c => c.Problems).Returns(problems);

            var handler = new SolveRequestMessageHandler(messenger.Object, context.Object);
            handler.HandleMessage(message, tcpClient.Object);

            Assert.Equal(ProblemStatus.New, problems[0].Status);
        }

        private static SolveRequestMessage GetMessage()
        {
            var message = new SolveRequestMessage
            {
                ProblemType = "DVRP",
                SolvingTimeout = 1000u,
                Data = new byte[5]
            };
            return message;
        }
    }
}
