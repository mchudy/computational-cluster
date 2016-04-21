using ComputationalCluster.DVRPTaskSolver.Problem;
using System.Linq;

namespace ComputationalCluster.DVRPTaskSolver.Algorithm
{
    public class ProblemMerger
    {

        public DVRPSolution MergeSolutions(DVRPSolution[] solutions)
        {
            return solutions.OrderBy(x => x.Cost).First();
        }
    }
}
