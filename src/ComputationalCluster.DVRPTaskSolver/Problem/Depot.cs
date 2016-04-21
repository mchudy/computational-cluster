using System;

namespace ComputationalCluster.DVRPTaskSolver.Problem
{
    [Serializable]
    public class Depot : Location
    {
        public int StartTime { get; set; }
        public int EndTime { get; set; }

        public Depot() : base()
        {

        }

        public Depot(int X, int Y, int StartHour, int EndHour) : base(X, Y)
        {
            this.StartTime = StartHour;
            this.EndTime = EndHour;
        }
    }
}
