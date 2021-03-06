﻿using ComputationalCluster.Common.Messages;
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
    public class SolutionRequestMessageHandlerTests
    {
        private readonly Mock<IServerMessenger> messenger = new Mock<IServerMessenger>();
        private readonly Mock<IServerContext> context = new Mock<IServerContext>();
        private readonly Mock<ITcpClient> tcpClient = new Mock<ITcpClient>();
        private readonly IList<ProblemInstance> problems = new List<ProblemInstance>();
        private readonly ProblemInstance problem = new ProblemInstance();
        private readonly SolutionRequestMessage message;

        public SolutionRequestMessageHandlerTests()
        {
            context.SetupGet(c => c.Problems).Returns(problems);
            context.SetupGet(c => c.BackupMessages).Returns(new ConcurrentQueue<Message>());
            context.SetupGet(c => c.IsPrimary).Returns(true);
            problem.Id = 1;
            problems.Add(problem);
            message = new SolutionRequestMessage
            {
                Id = 1
            };
        }

        [Fact]
        public void WhenFinalSolutionHasNotYetBeenComputed_ShouldReturnOngoingType()
        {
            problem.Status = ProblemStatus.Divided;
            var handler = new SolutionRequestMessageHandler(messenger.Object, context.Object);

            handler.HandleMessage(message, tcpClient.Object);

            messenger.Verify(m => m.SendMessages(
                        It.Is<IList<Message>>(msgs => VerifyType(msgs.Cast<SolutionMessage>().First(), SolutionType.Ongoing)),
                        It.IsAny<INetworkStream>()));
        }

        [Fact]
        public void WhenFinalSolutionHasBeenComputed_ShouldReturnFinalSolutionType()
        {
            problem.Status = ProblemStatus.Final;
            var handler = new SolutionRequestMessageHandler(messenger.Object, context.Object);

            handler.HandleMessage(message, tcpClient.Object);

            messenger.Verify(m => m.SendMessages(
                        It.Is<IList<Message>>(msgs => VerifyType(msgs.Cast<SolutionMessage>().First(), SolutionType.Final)),
                        It.IsAny<INetworkStream>()));
        }


        [Fact]
        public void WhenFinalSolutionHasBeenComputed_ShouldReturnFinalSolutionData()
        {
            problem.FinalSolution = new byte[] { 14, 15 };
            problem.Status = ProblemStatus.Final;
            var handler = new SolutionRequestMessageHandler(messenger.Object, context.Object);

            handler.HandleMessage(message, tcpClient.Object);

            messenger.Verify(m => m.SendMessages(
                    It.Is<IList<Message>>(msgs => msgs.Cast<SolutionMessage>().First().Solutions[0].Data.SequenceEqual(problem.FinalSolution)),
                    It.IsAny<INetworkStream>()));
        }

        private bool VerifyType(SolutionMessage msg, SolutionType type)
        {
            if (!HasSolutions(msg)) return false;
            return msg.Solutions[0].Type == type;
        }


        private static bool HasSolutions(SolutionMessage msg)
        {
            return msg.Solutions != null && msg.Solutions.Length != 0;
        }
    }
}
