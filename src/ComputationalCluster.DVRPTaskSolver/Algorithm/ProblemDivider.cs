using ComputationalCluster.DVRPTaskSolver.Problem;
using log4net;
using System.Collections.Generic;
using System.Diagnostics;

namespace ComputationalCluster.DVRPTaskSolver.Algorithm
{
    public class ProblemDivider
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ProblemDivider));
        private readonly DVRPProblemInstance problem;
        private readonly List<Partition> partitions;
        private readonly int threadCount;

        public ProblemDivider(DVRPProblemInstance problem, int threadCount)
        {
            this.threadCount = threadCount;
            this.problem = problem;
            partitions = new List<Partition>();
        }

        private void CreatePartitions()
        {
            foreach (var partition in new Partition().MakePartitions(problem.VehiclesCount))
            {
                Partition p = new Partition(problem.VehiclesCount) { truckClients = partition };
                partitions.Add(p);
            }
        }

        public List<Partition>[] DividePartitions()
        {
            List<Partition>[] ret = new List<Partition>[threadCount];
            Stopwatch stopwatch = Stopwatch.StartNew();
            CreatePartitions();

            logger.Info($"[Task Solver] Generated {partitions.Count} partitions for the problem");

            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = new List<Partition>();
            }
            for (int i = 0; i < partitions.Count; i++)
            {
                ret[i % threadCount].Add(partitions[i]);
            }
            stopwatch.Stop();
            logger.Info($"[Task Solver] Dividing problem time {stopwatch.Elapsed}");
            return ret;
        }
    }
}
