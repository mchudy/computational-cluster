using System;

namespace ComputationalCluster.DVRPTaskSolver.Problem
{
    [Serializable]
    public class Client : Location
    {
        public int AvailableTime { get; set; }
        public int UnloadTime { get; set; }
        public int DemandSize { get; set; }

        public Client() : base()
        {

        }

        public Client(int X, int Y, int AvailableTime, int UnloadTime, int RequestSize) : base(X, Y)
        {
            this.AvailableTime = AvailableTime;
            this.DemandSize = RequestSize;
            this.UnloadTime = UnloadTime;
        }
    }
}
