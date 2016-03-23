using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Objects;
using ComputationalCluster.Server.Configuration;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace ComputationalCluster.Server
{
    public class ServerContext : IServerContext
    {
        private int currentProblemId;
        private int currentComponentId;

        public ServerContext(IServerConfiguration configuration)
        {
            Configuration = configuration;
        }

        //TODO: dictionary for fast access?

        public IServerConfiguration Configuration { get; }

        public IList<TaskManager> TaskManagers { get; } = new List<TaskManager>();
        public bool IsPrimary => Configuration.Mode == ServerMode.Primary;
        public IList<ComputationalNode> Nodes { get; } = new List<ComputationalNode>();
        public IList<ProblemInstance> Problems { get; } = new List<ProblemInstance>();
        public List<BackupCommunicationServer> BackupServers { get; set; } = new List<BackupCommunicationServer>();
        public ConcurrentQueue<Message> BackupMessages { get; } = new ConcurrentQueue<Message>();

        // only for Backup Mode
        public int Id { get; set; }

        public int GetNextComponentId()
        {
            return Interlocked.Increment(ref currentComponentId);
        }

        public int GetNextProblemId()
        {
            return Interlocked.Increment(ref currentProblemId);
        }


    }
}
