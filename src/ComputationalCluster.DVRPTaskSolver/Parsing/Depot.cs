using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.DVRPTaskSolver.Parsing
{
    public class Depot
    {
        public Location Location { get; set; }
        public int StartHour { get; set; }
        public int EndHour { get; set; }
    }
}
