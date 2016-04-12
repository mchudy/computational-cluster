using ComputationalCluster.Common.Helpers;
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Objects;
using ComputationalCluster.Server.Configuration;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            if (!IsPrimary) SetLocalAddress(configuration);
        }

        public IServerConfiguration Configuration { get; }

        public bool IsPrimary => Configuration.Mode == ServerMode.Primary;

        public IList<TaskManager> TaskManagers { get; } = new List<TaskManager>();
        public IList<ComputationalNode> Nodes { get; } = new List<ComputationalNode>();
        public IList<ProblemInstance> Problems { get; } = new List<ProblemInstance>();
        public List<BackupServer> BackupServers { get; set; } = new List<BackupServer>();

        public ConcurrentQueue<Message> BackupMessages { get; } = new ConcurrentQueue<Message>();

        /// <summary>
        /// Informs whether the server address in Configuration property has been 
        /// initialized to the proper value (i.e. if the server is a 3rd it should
        /// be equal to the 2nd server address)
        /// </summary>
        public bool IsMasterServerSet { get; set; }

        // only for Backup Mode
        public int Id { get; set; }

        public string LocalAddress { get; set; }

        public int GetNextComponentId()
        {
            return Interlocked.Increment(ref currentComponentId);
        }

        public int GetNextProblemId()
        {
            return Interlocked.Increment(ref currentProblemId);
        }

        public NoOperationMessage GetNoOperationMessage()
        {
            return new NoOperationMessage
            {
                BackupCommunicationServers = BackupServers.Select(b => new BackupCommunicationServer
                {
                    Address = b.Address,
                    Port = b.Port
                }).ToList()
            };
        }

        private void SetLocalAddress(IServerConfiguration configuration)
        {
            // special case when servers are running on localhost
            if (IPAddress.IsLoopback(IPAddress.Parse(configuration.ServerAddress)))
            {
                LocalAddress = configuration.ServerAddress;
            }
            else
            {
                LocalAddress = IPHelper.GetLocalIPAddress().ToString();
            }
        }
    }

    public class BackupServer : BackupCommunicationServer
    {
        public int Id { get; set; }
    }
}
