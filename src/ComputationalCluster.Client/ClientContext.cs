using ComputationalCluster.Common.Objects;
using System.Collections.Generic;
using System.Diagnostics;

namespace ComputationalCluster.Client
{
    public class ClientContext
    {
        public int WaitTime { get; set; } = 8;
        public IList<BackupCommunicationServer> BackupServers { get; set; } = new List<BackupCommunicationServer>();
        public int? CurrentProblemId { get; set; }
        public Stopwatch Stopwatch { get; set; }
    }
}
