using System;

namespace ComputationalCluster.DVRPTaskSolver.Problem
{
    [Serializable]
    public class DVRPProblemInstance
    {
        public int VehiclesCount { get; set; }
        public int VehicleCapacity { get; set; }
        public int VehicleSpeed { get; set; }
        public int CutoffTime { get; set; }
        public Client[] Clients { get; set; }
        public Depot[] Depots { get; set; }
    }
}
