using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.DVRPTaskSolver.Parsing
{
    public class Client : Location
    {
        public int AvailableTime { get; set; }
        public int UnloadTime { get; set; }
        public int RequestSize { get; set; }

        public Client(int X, int Y, int AvailableTime, int UnloadTime, int RequestSize) : base(X,Y)
        {
            this.AvailableTime = AvailableTime;
            this.RequestSize = RequestSize;
            this.UnloadTime = UnloadTime;
        }
    }
}
