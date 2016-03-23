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

        public StatusChecker(IServerContext context)
        {
            this.context = context;
        }

        public void Add(ComputationalNode node)
        {
            Task.Run(() => CheckNodeTimeout(node));
        }

        public void Add(TaskManager manager)
        {
            Task.Run(() => CheckTaskManagerTimeout(manager));
        }

        private void CheckNodeTimeout(ComputationalNode node)
        {
            while (true)
            {
                Thread.Sleep((int)(context.Configuration.Timeout * 1000));
                //TODO: ensure atomicity
                if (!node.ReceivedStatus)
                {
                    logger.Error($"FAILURE - node with id {node.Id}");
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
                    //TODO: proper deregistration handling
                    context.Nodes.Remove(node);
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
                    logger.Error($"FAILURE - task manager with id {manager.Id}");
                    break;
                }
                manager.ReceivedStatus = false;
            }
        }
    }
}
