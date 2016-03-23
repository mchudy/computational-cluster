﻿using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace ComputationalCluster.TaskManager.Tests
{
    public class TaskManagerTests
    {
        [Fact]
        public void TaskManager_ShouldSendRegisterMessageOnStart()
        {
            var messenger = new Mock<IMessenger>();
            messenger.Setup(m => m.SendMessage(It.IsAny<Message>()))
                .Returns(new List<Message> { new RegisterResponseMessage() });
            var node = new TaskManager(messenger.Object, new TaskManagerContext());

            node.Start();

            messenger.Verify(m => m.SendMessage(It.Is<RegisterMessage>(msg => msg.Type.Type == ClientComponentType.TaskManager)),
                Times.Once());
        }
    }
}
