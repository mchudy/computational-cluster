using System.Configuration;
using Xunit;

namespace ComputationalCluster.Common.Tests
{
    public class ConfigurationTests
    {
        private const string addressKey = "ServerAddress";
        private const string portKey = "ServerPort";
        private const string correctAddress = "192.168.1.1";
        private const int correctPort = 9000;

        [Fact]
        public void WhenIncorrectPortNumber_ShouldThrow()
        {
            ConfigurationManager.AppSettings[addressKey] = correctAddress;
            ConfigurationManager.AppSettings[portKey] = 70000.ToString();

            Assert.Throws<BadConfigException>(() => new Configuration());
        }

        [Fact]
        public void ShouldReadDataFromAppConfig()
        {
            ConfigurationManager.AppSettings[addressKey] = correctAddress;
            ConfigurationManager.AppSettings[portKey] = correctPort.ToString();

            var configuration = new Configuration();

            Assert.Equal(configuration.ServerAddress, correctAddress);
            Assert.Equal(configuration.ServerPort, correctPort);
        }
    }
}
