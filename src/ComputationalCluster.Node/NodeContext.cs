using ComputationalCluster.Common.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.Node
{
    public class NodeContext
    {
        public string CurrentProblemType { get; set; }
        public int CurrentId { get; set; }
        public ulong SolvingTimeout { get; set; }
        public IList<PartialProblem> CurrentPartialProblems { get; set; }
        public IList<Solution> CurrentSolutions { get; set; }
    }
}
