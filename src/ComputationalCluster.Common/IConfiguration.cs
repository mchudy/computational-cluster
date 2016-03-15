using System.Net;

namespace ComputationalCluster.Common
{
    public interface IConfiguration
    {
        IPAddress ServerAddress { get; set; }
        int ServerPort { get; set; }
    }
}