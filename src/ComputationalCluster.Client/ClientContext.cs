using ComputationalCluster.Common.Objects;
using System.Collections.Generic;
using System.Diagnostics;

namespace ComputationalCluster.Client
{
    public class ClientContext
    {
        public string ProblemFileName { get; set; } = string.Empty;
        public int WaitTime { get; set; } = 1;
        public IList<BackupCommunicationServer> BackupServers { get; set; } = new List<BackupCommunicationServer>();
        public int? CurrentProblemId { get; set; }
        public Stopwatch Stopwatch { get; set; }
    }
}
