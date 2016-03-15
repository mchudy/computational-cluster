using System;
using System.Configuration;
using System.Net;

namespace ComputationalCluster.Common
{
    public class Configuration : IConfiguration
    {
        public Configuration()
        {
            ServerAddress = ConfigurationManager.AppSettings[nameof(ServerAddress)];
            LoadPortNumber();
        }

        public string ServerAddress { get; set; }
        public int ServerPort { get; set; }

        private void LoadPortNumber()
        {
            int port;
            if (!int.TryParse(ConfigurationManager.AppSettings[nameof(ServerPort)], out port) ||
                !IsValidPort(port))
            {
                throw new BadConfigException("Incorrect port number in configuration file");
            }
            ServerPort = port;
        }

        private bool IsValidPort(int port)
        {
            return port >= IPEndPoint.MinPort && port <= IPEndPoint.MaxPort;
        }
    }

    public class BadConfigException : Exception
    {
        public BadConfigException(string message)
            : base(message)
        {
        }
    }
}
