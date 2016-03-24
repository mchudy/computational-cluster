using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Networking;
using ComputationalCluster.Common.Objects;
using ComputationalCluster.Server.Handlers;
using Moq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ComputationalCluster.Server.Tests
{
    public class PartialProblemMessageHandlerTests
    {
        private readonly Mock<IServerContext> context = new Mock<IServerContext>();
        private readonly Mock<ITcpClient> tcpClient = new Mock<ITcpClient>();
        private readonly PartialProblemsMessage message;


        public PartialProblemMessageHandlerTests()
        {
            context.SetupGet(c => c.Problems).Returns(new List<ProblemInstance>());
            message = GetMessage();
            context.SetupGet(c => c.BackupMessages).Returns(new ConcurrentQueue<Message>());
        }

        [Fact]
        public void ShouldChangeProblemStatusToDivided()
        {
            var problems = new List<ProblemInstance>();
            var problem = new ProblemInstance
            {
                Id = 1,
            };
            problem.PartialProblems = new PartialProblemInstance[1];
            problems.Add(problem);
            context.SetupGet(c => c.Problems).Returns(problems);

            var handler = new PartialProblemMessageHandler(context.Object);
            handler.HandleMessage(message, tcpClient.Object);

            Assert.Equal(ProblemStatus.Divided, problems[0].Status);
            Assert.Equal(PartialProblemState.New, problems[0].PartialProblems[0].State);
        }

        private static PartialProblemsMessage GetMessage()
        {
            var message = new PartialProblemsMessage
            {
                Id = 1,
                ProblemType = "DVRP",
                SolvingTimeout = 1000u,
                CommonData = new byte[3],
                PartialProblems = new PartialProblem[1]
            };
            return message;
        }
    }
}
