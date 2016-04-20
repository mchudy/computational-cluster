using log4net;
using System;
using System.Text;

namespace ComputationalCluster.DVRPTaskSolver
{
    public class DVRPTaskSolver : UCCTaskSolver.TaskSolver
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(DVRPTaskSolver));

        public override string Name => "DVRP";

        public DVRPTaskSolver(byte[] problemData)
            : base(problemData)
        {
            if (problemData == null) return;
        }

        public override byte[][] DivideProblem(int threadCount)
        {
            logger.Info("[Task Solver] Dividing problem");
            var partials = new byte[threadCount][];
            for (int i = 0; i < threadCount; i++)
            {
                partials[i] = new byte[] { 1, 1 };
            }
            return partials;
        }

        public override byte[] MergeSolution(byte[][] solutions)
        {
            logger.Info("[Task Solver] Merging solution");
            return Encoding.UTF8.GetBytes("Hello!");
        }

        public override byte[] Solve(byte[] partialData, TimeSpan timeout)
        {
            logger.Info("[Task Solver] Solving partial problem");
            return new byte[] { 1, 1 };
        }
    }
}
