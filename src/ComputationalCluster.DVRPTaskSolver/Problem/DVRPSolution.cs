using System;
using System.Collections.Generic;

namespace ComputationalCluster.DVRPTaskSolver.Problem
{
    [Serializable]
    public class DVRPSolution
    {
        public double Cost { get; set; }
        public List<int>[] Routes { get; set; }
    }
}
