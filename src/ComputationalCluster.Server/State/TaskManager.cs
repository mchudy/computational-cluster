using System.Collections.Generic;

namespace ComputationalCluster.Server
{
    public class TaskManager : ClientComponent
    {
        public IList<string> SolvableProblems { get; set; } = new List<string>();
        public int ThreadsCount { get; set; }
        public bool ReceivedStatus { get; set; }
    }
}