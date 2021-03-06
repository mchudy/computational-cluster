﻿using ComputationalCluster.Client.Handlers;
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Objects;
using Moq;
using System;
using System.Reflection;
using System.Text;
using Xunit;

namespace ComputationalCluster.Client.Tests
{
    public class ClientTests
    {
        [Fact]
        public void Client_ShouldSendSolveRequestMessageOnStart()
        {
            var messenger = new Mock<IMessenger>();
            var node = new Client(messenger.Object, new Mock<ClientContext>().Object);

            //TODO: Filesystem wrapper!
            node.Start(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);

            messenger.Verify(m => m.SendMessage(It.Is<SolveRequestMessage>(msg => msg.ProblemType == "DVRP")),
                Times.Once());
        }

        [Fact]
        public void Client_ShouldUpdateBackupServers()
        {
            var context = new Mock<ClientContext>();
            NoOperationMessageHandler hndl = new NoOperationMessageHandler(context.Object);
            var noOp = new Mock<NoOperationMessage>();

            hndl.HandleResponse(noOp.Object);

            Assert.Equal(context.Object.BackupServers, noOp.Object.BackupCommunicationServers);

        }

        [Fact]
        public void ShouldNullCurrProblemId()
        {
            var context = new Mock<ClientContext>();
            SolutionsMessageHandler hndl = new SolutionsMessageHandler(context.Object);
            var msg = new Mock<SolutionMessage>();

            msg.Object.Solutions = new Solution[1] { new Solution() { Type = SolutionType.Final, Data = Encoding.UTF8.GetBytes("1\n2\n2\n") } };
            context.Object.CurrentProblemId = 1;

            hndl.HandleResponse(msg.Object);

            Assert.Equal(context.Object.CurrentProblemId, null);

        }
    }
}