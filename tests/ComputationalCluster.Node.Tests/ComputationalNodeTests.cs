using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using Moq;
using Xunit;

namespace ComputationalCluster.Node.Tests
{
    public class ComputationalNodeTests
    {
        [Fact]
        public void Node_ShouldSendRegisterMessageOnStart()
        {
            var messenger = new Mock<IMessenger>();
            var node = new ComputationalNode(messenger.Object);

            node.Start();

            messenger.Verify(m => m.SendMessage(It.Is<RegisterMessage>(msg => msg.Type == RegisterType.ComputationalNode)),
                Times.Once());
        }
    }
}
