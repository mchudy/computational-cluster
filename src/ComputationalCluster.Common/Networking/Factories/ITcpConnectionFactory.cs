namespace ComputationalCluster.Common.Networking
{
    public interface ITcpClientFactory
    {
        ITcpClient Create();
    }
}