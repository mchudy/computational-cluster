namespace ComputationalCluster.Common.Networking
{
    public interface ITcpConnectionFactory
    {
        ITcpConnection Create();
    }
}