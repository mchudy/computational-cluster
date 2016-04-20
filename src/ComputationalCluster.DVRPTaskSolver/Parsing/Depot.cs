using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.DVRPTaskSolver.Parsing
{
    public class Depot : Location
    {
        public int StartTime { get; set; }
        public int EndTime { get; set; }

        public Depot() : base()
        {

        }

        public Depot(int X, int Y, int StartHour, int EndHour) : base(X,Y)
        {
            this.StartTime = StartHour;
            this.EndTime = EndHour;
        }
    }
}
