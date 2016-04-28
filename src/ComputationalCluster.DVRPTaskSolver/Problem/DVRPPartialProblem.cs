using ComputationalCluster.DVRPTaskSolver.Algorithm;
using System;
using System.Collections.Generic;

namespace ComputationalCluster.DVRPTaskSolver.Problem
{
    [Serializable]
    public class DVRPPartialProblem
    {
        public DVRPProblemInstance ProblemInstance { get; set; }
        public List<Partition> Partitions { get; set; }
    }
}
