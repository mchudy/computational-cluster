using System;

namespace ComputationalCluster.DVRPTaskSolver
{
    public class DVRPTaskSolver : UCCTaskSolver.TaskSolver
    {
        public override string Name => "DVRP";

        public DVRPTaskSolver(byte[] problemData)
            : base(problemData)
        {
        }

        public override byte[][] DivideProblem(int threadCount)
        {
            throw new NotImplementedException();
        }

        public override byte[] MergeSolution(byte[][] solutions)
        {
            throw new NotImplementedException();
        }

        public override byte[] Solve(byte[] partialData, TimeSpan timeout)
        {
            throw new NotImplementedException();
        }
    }
}
