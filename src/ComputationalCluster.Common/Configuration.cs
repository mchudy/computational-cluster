using System.Configuration;

namespace ComputationalCluster.Common
{
    public class Configuration : IConfiguration
    {
        public Configuration()
        {
            ServerAddress = ConfigurationManager.AppSettings[nameof(ServerAddress)];
            ServerPort = int.Parse(ConfigurationManager.AppSettings[nameof(ServerPort)]);
        }

        public string ServerAddress { get; set; }
        public int ServerPort { get; set; }
    }
}
