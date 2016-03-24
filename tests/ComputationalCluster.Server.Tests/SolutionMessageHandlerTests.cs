using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Networking;
using ComputationalCluster.Common.Objects;
using ComputationalCluster.Server.BackupHandlers;
using Moq;
using System.Collections.Concurrent;
using System.Collections.Generic;

using Xunit;

namespace ComputationalCluster.Server.Tests
{
    public class SolutionMessageHandlerTests
    {
        private readonly Mock<IServerContext> context = new Mock<IServerContext>();
        private readonly Mock<ITcpClient> tcpClient = new Mock<ITcpClient>();

        public SolutionMessageHandlerTests()
        {
            context.SetupGet(c => c.Problems).Returns(new List<ProblemInstance>());
            context.SetupGet(c => c.BackupMessages).Returns(new ConcurrentQueue<Message>());
        }

        [Fact]
        public void ShouldChangePartialProblemStatusToComputed()
        {
            var problems = GetProblems();
            context.SetupGet(c => c.Problems).Returns(problems);
            var message = GetMessageWithSolutionPartialStatus();
            var handler = new SolutionMessageHandler(context.Object);
            handler.HandleResponse(message);

            Assert.Equal(PartialProblemState.Computed, problems[0].PartialProblems[0].State);
        }

        [Fact]
        public void ShouldChangeProblemStatusToFinal()
        {
            var problems = GetProblems();
            context.SetupGet(c => c.Problems).Returns(problems);
            var message = GetMessageWithSolutionFinalStatus();
            var handler = new SolutionMessageHandler(context.Object);
            handler.HandleResponse(message);

            Assert.Equal(ProblemStatus.Final, problems[0].Status);
        }

        private static List<ProblemInstance> GetProblems()
        {
            var problems = new List<ProblemInstance>();
            problems.Add( new ProblemInstance
            {
                Id = 1,
                PartialProblems = new PartialProblemInstance[]
                {
                    new PartialProblemInstance
                    {
                        Problem = new PartialProblem
                        {
                            TaskId = 1,
                        }
                    }
                }
            });
         
            return problems;
        }

        private static SolutionMessage GetMessageWithSolutionPartialStatus()
        {
            var message = new SolutionMessage
            {
                Id = 1,
                ProblemType = "DVRP",
                CommonData = new byte[3],
                Solutions = new Solution[1]
                {
                    new Solution
                    {
                        TaskId = 1,
                        Type = SolutionType.Partial
                    }
                }
            };
            return message;
        }

        private static SolutionMessage GetMessageWithSolutionFinalStatus()
        {
            var message = new SolutionMessage
            {
                Id = 1,
                ProblemType = "DVRP",
                CommonData = new byte[3],
                Solutions = new Solution[1]
                {
                    new Solution
                    {
                        TaskId = 1,
                        Type = SolutionType.Final
                    }
                }
            };
            return message;
        }
    }
}
