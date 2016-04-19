using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.DVRPTaskSolver.Parsing
{
    public class Depot : Location
    {
        public int StartHour { get; set; }
        public int EndHour { get; set; }

        public Depot() : base()
        {

        }

        public Depot(int X, int Y, int StartHour, int EndHour) : base(X,Y)
        {
            this.StartHour = StartHour;
            this.EndHour = EndHour;
        }
    }
}
