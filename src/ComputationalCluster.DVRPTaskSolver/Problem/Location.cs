using System;

namespace ComputationalCluster.DVRPTaskSolver.Problem
{
    [Serializable]
    public class Location
    {

        public int X { get; set; }
        public int Y { get; set; }

        public Location()
        {

        }

        public Location(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }

}
