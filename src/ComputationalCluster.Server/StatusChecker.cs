using ComputationalCluster.Common.Messages;
using log4net;
using System.Threading;
using System.Threading.Tasks;

namespace ComputationalCluster.Server
{
    //TODO: use DateTime somehow, one thread only?
    public class StatusChecker : IStatusChecker
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(StatusChecker));

        private readonly IServerContext context;
        private readonly IServerMessenger messenger;

        public StatusChecker(IServerContext context, IServerMessenger messenger)
        {
            this.context = context;
            this.messenger = messenger;
        }

        public void Add(ComputationalNode node)
        {
            if (context.IsPrimary)
            {
                Task.Run(() => CheckNodeTimeout(node));
            }
        }

        public void Add(BackupServer server)
        {
            if (context.IsPrimary)
            {
                Task.Run(() => CheckBackupTimeout(server));
            }
        }

        public void Add(TaskManager manager)
        {
            if (context.IsPrimary)
            {
                Task.Run(() => CheckTaskManagerTimeout(manager));
            }
        }

        private void CheckNodeTimeout(ComputationalNode node)
        {
            while (true)
            {
                Thread.Sleep((int)(context.Configuration.Timeout * 1000));
                if (!node.ReceivedStatus)
                {
                    logger.Warn($"FAILURE - node with id {node.Id}");
                    foreach (var problem in context.Problems)
                    {
                        foreach (var partial in problem.PartialProblems)
                        {
                            if (partial.NodeId == node.Id)
                            {
                                partial.State = PartialProblemState.New;
                                logger.Info($"Resetting status of partial {partial.Problem.TaskId} from problem {problem.Id} to new");
                            }
                        }
                    }
                    context.Nodes.Remove(node);
                    var deregisterMessage = new RegisterMessage
                    {
                        Deregister = true,
                        DeregisterSpecified = true,
                        Id = (ulong)node.Id
                    };
                    //messenger.SendToBackup(deregisterMessage);
                    context.BackupMessages.Enqueue(deregisterMessage);
                    break;
                }
                node.ReceivedStatus = false;
            }
        }

        private void CheckTaskManagerTimeout(TaskManager manager)
        {
            while (true)
            {
                Thread.Sleep((int)(context.Configuration.Timeout * 1000));
                if (!manager.ReceivedStatus)
                {
                    context.TaskManagers.Remove(manager);
                    logger.Warn($"FAILURE - task manager with id {manager.Id}");
                    break;
                }
                manager.ReceivedStatus = false;
            }
        }

        private void CheckBackupTimeout(BackupServer backup)
        {
            while (true)
            {
                Thread.Sleep((int)(context.Configuration.Timeout * 1000));
                if (!backup.ReceivedStatus)
                {
                    context.BackupServers.Clear();
                    logger.Warn($"FAILURE - backup with id {backup.Id}");
                    break;
                }
                backup.ReceivedStatus = false;
            }
        }
    }
}
