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
        public IList<ComputationalNode> Nodes { get; } = new List<ComputationalNode>();
        public IList<ProblemInstance> Problems { get; } = new List<ProblemInstance>();
        public List<BackupCommunicationServer> BackupServers { get; } = new List<BackupCommunicationServer>();

        public ConcurrentQueue<ProblemInstance> ProblemQueue { get; } = new ConcurrentQueue<ProblemInstance>();

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
