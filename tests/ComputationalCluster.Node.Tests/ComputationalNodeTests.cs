﻿using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using log4net;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace ComputationalCluster.Node.Tests
{
    public class ComputationalNodeTests
    {
        [Fact]
        public void Node_ShouldSendRegisterMessageOnStart()
        {
            var logger = new Mock<ILog>();
            var messenger = new Mock<IMessenger>();
            messenger.Setup(m => m.SendMessage(It.IsAny<Message>()))
                .Returns(new List<Message> { new RegisterResponseMessage() });
            var node = new ComputationalNode(messenger.Object) { Logger = logger.Object };

            node.Start();

            messenger.Verify(m => m.SendMessage(It.Is<RegisterMessage>(msg => msg.Type == RegisterType.ComputationalNode)),
                Times.Once());
        }
    }
}
