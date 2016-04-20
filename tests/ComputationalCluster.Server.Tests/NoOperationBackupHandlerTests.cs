using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Objects;
using ComputationalCluster.Server.BackupHandlers;
using ComputationalCluster.Server.Configuration;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ComputationalCluster.Server.Tests
{
    public class NoOperationBackupHandlerTests
    {
        private readonly string primaryServerAddress = "192.168.1.1";
        private readonly int primaryServerPort = 1000;

        private const string backupServerAddress = "192.168.1.2";
        private const ushort backupServerPort = 9000;

        private readonly string localAddress = "192.168.1.3";
        private readonly ushort localPort = 9001;

        private readonly Mock<IServerConfiguration> config = new Mock<IServerConfiguration>();
        private readonly Mock<IServerContext> context = new Mock<IServerContext>();
        private readonly NoOperationMessageHandler handler;

        public NoOperationBackupHandlerTests()
        {
            config.SetupAllProperties();
            config.SetupGet(c => c.ServerPort).Returns(primaryServerPort);
            config.SetupGet(c => c.ServerAddress).Returns(primaryServerAddress);
            config.SetupGet(c => c.ListeningPort).Returns(localPort);

            context.SetupAllProperties();
            context.SetupGet(c => c.BackupServers).Returns(
                new List<BackupServer> { new BackupServer { Address = localAddress, Port = localPort } });
            context.SetupGet(c => c.Configuration).Returns(config.Object);
            context.Setup(c => c.LocalAddress).Returns(localAddress);

            handler = new NoOperationMessageHandler(context.Object);
        }

        [Fact]
        public void ShouldSetIsMasterServerSetPropertyToTrue()
        {
            handler.HandleResponse(GetMessage(
                new BackupServer { Address = localAddress, Port = localPort }));

            Assert.True(context.Object.IsMasterServerSet);
        }

        [Fact]
        public void WhenIsMasterServerAlreadySet_ShouldNotTryToSetProperties()
        {
            context.SetupGet(c => c.IsMasterServerSet).Returns(true);

            handler.HandleResponse(GetMessage());

            context.VerifySet(c => c.IsMasterServerSet = It.IsAny<bool>(), Times.Never());
            config.VerifySet(c => c.ServerPort = It.IsAny<int>(), Times.Never());
            config.VerifySet(c => c.ServerAddress = It.IsAny<string>(), Times.Never());
        }

        [Fact]
        public void WhenIsFirstBackupServer_ShouldKeepThePrimaryServerAddress()
        {
            handler.HandleResponse(GetMessage());

            Assert.Equal(primaryServerAddress, config.Object.ServerAddress);
            Assert.Equal(primaryServerPort, config.Object.ServerPort);
        }

        [Fact]
        public void WhenIsSecondBackupServer_ShouldChangeTheServerAddressToThisOfFirstBackupServer()
        {
            context.SetupGet(c => c.BackupServers).Returns(new List<BackupServer>
            {
                new BackupServer {Address = backupServerAddress, Port = backupServerPort},
                new BackupServer { Address = localAddress, Port = localPort },

            });

            var message = GetMessage(
                new BackupServer { Address = backupServerAddress, Port = backupServerPort },
                new BackupServer { Address = localAddress, Port = localPort }
            );

            handler.HandleResponse(message);

            config.VerifySet(c => c.ServerAddress = It.Is<string>(s => s == backupServerAddress), Times.Once);
            config.VerifySet(c => c.ServerPort = It.Is<int>(p => p == backupServerPort), Times.Once);
        }

        [Fact]
        public void WhenIsSecondBackupServerOnTheSameLocalAddress_ShouldChangeTheServerAddressToThisOfFirstBackupServer()
        {
            context.SetupGet(c => c.BackupServers).Returns(new List<BackupServer>
            {
                new BackupServer {Address = "127.0.0.1", Port = backupServerPort},
                new BackupServer { Address = "127.0.0.1", Port = localPort },
            });
            context.SetupGet(c => c.LocalAddress).Returns("127.0.0.1");
            var message = GetMessage(
                new BackupServer { Address = "127.0.0.1", Port = backupServerPort },
                new BackupServer { Address = "127.0.0.1", Port = localPort }
            );

            handler.HandleResponse(message);

            config.VerifySet(c => c.ServerAddress = It.Is<string>(s => s == "127.0.0.1"), Times.Once);
            config.VerifySet(c => c.ServerPort = It.Is<int>(p => p == backupServerPort), Times.Once);
        }

        private NoOperationMessage GetMessage(params BackupCommunicationServer[] servers)
        {
            return new NoOperationMessage
            {
                BackupCommunicationServers = servers.ToList()
            };
        }
    }
}
