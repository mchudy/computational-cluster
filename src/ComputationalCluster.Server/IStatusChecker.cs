namespace ComputationalCluster.Server
{
    public interface IStatusChecker
    {
        void Add(TaskManager manager);
        void Add(ComputationalNode node);
        void Add(BackupServer server);
    }
}