using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.DVRPTaskSolver.Parsing
{
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
