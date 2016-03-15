namespace ComputationalCluster.Server.Configuration
{
    public interface IServerConfiguration
    {
        int ListeningPort { get; set; }
        string MasterServerAddress { get; set; }
        int MasterServerPort { get; set; }
        ServerMode Mode { get; set; }
        uint Timeout { get; set; }
    }
}