using ComputationalCluster.DVRPTaskSolver.Algorithm;
using ComputationalCluster.DVRPTaskSolver.Parsing;
using ComputationalCluster.DVRPTaskSolver.Problem;
using log4net;
using System;
using System.Collections.Generic;

namespace ComputationalCluster.DVRPTaskSolver
{
    public class DVRPTaskSolver : UCCTaskSolver.TaskSolver
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(DVRPTaskSolver));
        private readonly IDVRPParser parser = new DVRPParser();
        private readonly PartialProblemsSerializer partialProblemsSerializer = new PartialProblemsSerializer();
        private readonly SolutionsSerializer solutionsSerializer = new SolutionsSerializer();

        public override string Name => "DVRP";

        public DVRPTaskSolver(byte[] problemData)
            : base(problemData)
        {
            if (problemData == null) return;
        }

        public override byte[][] DivideProblem(int threadCount)
        {
            logger.Info("[Task Solver] Dividing problem");

            DVRPProblemInstance problemInstance = parser.Parse(_problemData);
            var divider = new ProblemDivider(problemInstance, threadCount);
            List<Partition>[] partitions = divider.DividePartitions();
            return partialProblemsSerializer.Serialize(problemInstance, partitions);
        }

        public override byte[] MergeSolution(byte[][] solutionsData)
        {
            logger.Info("[Task Solver] Merging solution");
            DVRPSolution[] solutions = solutionsSerializer.Deserialize(solutionsData);
            DVRPSolution finalSolution = new ProblemMerger().MergeSolutions(solutions);
            return solutionsSerializer.SerializeForClient(finalSolution);
        }

        public override byte[] Solve(byte[] partialData, TimeSpan timeout)
        {
            logger.Info("[Task Solver] Solving partial problem");

            DVRPPartialProblem partialProblem = partialProblemsSerializer.Deserialize(partialData);
            var solution = new DVRPSolver(partialProblem).Solve();
            return solutionsSerializer.Serialize(solution);
        }
    }
}
