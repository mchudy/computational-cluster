namespace ComputationalCluster.Common
{
    public interface IConfiguration
    {
        string ServerAddress { get; set; }
        int ServerPort { get; set; }
    }
}