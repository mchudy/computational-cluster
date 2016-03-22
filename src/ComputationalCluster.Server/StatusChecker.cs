using log4net;
using System.Threading;
using System.Threading.Tasks;

namespace ComputationalCluster.Server
{
    //TODO: use DateTime somehow?
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
                    //TODO: proper deregistration handling
                    context.Nodes.Remove(node);
                    logger.Error($"FAILURE - node with id {node.Id}");
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
