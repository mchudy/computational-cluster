namespace ComputationalCluster.Common.Networking.Factories
{
    public interface IMessageStreamFactory
    {
        IMessageStreamReader CreateReader(INetworkStream stream);
        IMessageStreamWriter CreateWriter(INetworkStream stream);
    }
}