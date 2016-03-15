using System;
using System.Configuration;
using System.Net;

namespace ComputationalCluster.Common
{
    public class Configuration : IConfiguration
    {
        public Configuration()
        {
            LoadAddress();
            LoadPortNumber();
        }

        public IPAddress ServerAddress { get; set; }
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

        private void LoadAddress()
        {
            IPAddress address;
            if (!IPAddress.TryParse(ConfigurationManager.AppSettings[nameof(ServerAddress)], out address))
            {
                throw new BadConfigException("Incorrect IP address in configuration file");
            }
            ServerAddress = address;
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
