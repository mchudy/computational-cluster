using ComputationalCluster.Common.Objects;
using System.Collections.Generic;

namespace ComputationalCluster.Node
{
    //TODO: should keep track of all the threads
    public class NodeContext
    {
        public string CurrentProblemType { get; set; }
        public int CurrentId { get; set; }
        public ulong SolvingTimeout { get; set; }
        public IList<PartialProblem> CurrentPartialProblems { get; set; }
        public IList<Solution> CurrentSolutions { get; set; }
    }
}
