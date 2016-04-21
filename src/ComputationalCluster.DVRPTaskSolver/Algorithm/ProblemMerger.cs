using ComputationalCluster.DVRPTaskSolver.Problem;
using System.Collections.Generic;
using System.Linq;

namespace ComputationalCluster.DVRPTaskSolver.Algorithm
{
    public class ProblemMerger
    {
        
        public DVRPSolution MergeSolutions(DVRPSolution[] solutions)
        {
            solutions.GroupBy(x => x.Cost);
            return solutions[0];
        }


    }
}
