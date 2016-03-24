using ComputationalCluster.Common;

namespace ComputationalCluster.Server.Configuration
{
    public interface IServerConfiguration : IConfiguration
    {
        int ListeningPort { get; set; }
        ServerMode Mode { get; set; }
        uint Timeout { get; set; }
    }
}