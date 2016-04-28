using ComputationalCluster.DVRPTaskSolver.Algorithm;
using ComputationalCluster.DVRPTaskSolver.Problem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ComputationalCluster.DVRPTaskSolver.Tests
{
    public class AlgorithmTests
    {
        private DVRPProblemInstance simpleProblem1;
        private DVRPProblemInstance simpleProblem2;
        private DVRPProblemInstance problemInstance4;
        private DVRPProblemInstance problemInstance5;
        private int threadCount;

        public AlgorithmTests()
        {
            simpleProblem1 = new DVRPProblemInstance
            {
                Depots = new Depot[] { new Depot(0, 0, 0, 500) },
                Clients = new Client[] { new Client(-50, 0, 400, 20, 50), new Client(50, 0, 400, 20, 50) },
                VehicleCapacity = 80,
                VehiclesCount = 2
            };

            simpleProblem2 = new DVRPProblemInstance
            {
                Depots = new Depot[] { new Depot(0, 0, 0, 100) },
                Clients = new Client[] { new Client(-50, 0, 80, 20, 50), new Client(50, 0, 80, 20, 50) },
                VehicleCapacity = 80,
                VehiclesCount = 2
            };

            problemInstance4 = new DVRPProblemInstance
            {
                Depots = new Depot[] { new Depot(0, 0, 0, 480) },
                Clients = new Client[] { new Client(-50, 10, 243, 20, 28), new Client(19, 53, 157, 20, 26),
                                         new Client(88, -43, 230, 20, 2), new Client(-40, 5, 427, 20, 25) },
                VehicleCapacity = 100,
                VehiclesCount = 4
            };

            problemInstance5 = new DVRPProblemInstance
            {
                Depots = new Depot[] { new Depot(0, 0, 0, 500) },
                Clients = new Client[] { new Client(14, -29, 391, 20, 47), new Client(21, 51, 329, 20, 28),
                                         new Client(35, -10, 442, 20, 15), new Client(-90, -8, 64, 20, 29),
                                         new Client(40, -69, 126, 20, 26)},
                VehicleCapacity = 100,
                VehiclesCount = 5
            };

            threadCount = 4;
        }

        [Fact]
        void Solve_ShouldSolveSimpleProblem1()
        {
            var ret = Solve(simpleProblem1);

            SolutionsSerializer ss = new SolutionsSerializer();
            string solutionString = Encoding.UTF8.GetString(ss.Serialize(ret));

            Assert.NotNull(ret);
            Assert.Equal((int)ret.Cost, 200);
            Assert.Contains("[0,2,0,1,0]", solutionString);
        }

        [Fact]
        void Solve_ShouldSolveSimpleProblem2()
        {
            var ret = Solve(simpleProblem2);

            SolutionsSerializer ss = new SolutionsSerializer();
            string solutionString = Encoding.UTF8.GetString(ss.Serialize(ret));

            Assert.NotNull(ret);
            Assert.Equal((int)ret.Cost, 200);
            Assert.Contains("[0,2,0]", solutionString);
            Assert.Contains("[0,1,0]", solutionString);
        }

        [Fact]
        void Solve_ShouldSolve4Client1Depot()
        {
            var ret = Solve(problemInstance4);

            SolutionsSerializer ss = new SolutionsSerializer();
            string solutionString = Encoding.UTF8.GetString(ss.Serialize(ret));

            Assert.NotNull(ret);
            Assert.Equal((int)ret.Cost, 374);
            Assert.Contains("4,1", solutionString);
            Assert.Contains("3,2", solutionString);
        }

        [Fact]
        void Solve_ShouldSolve5Client1Depot()
        {
            var ret = Solve(problemInstance5);

            SolutionsSerializer ss = new SolutionsSerializer();
            string solutionString = Encoding.UTF8.GetString(ss.Serialize(ret));

            Assert.NotNull(ret);
            Assert.Equal((int)ret.Cost, 446);
            Assert.Contains("5,1,3", solutionString);
            Assert.Contains("4,2", solutionString);
        }

      

        private DVRPSolution Solve(DVRPProblemInstance problemInstance)
        {
            ProblemDivider pd = new ProblemDivider(problemInstance, threadCount);
            var partitions = pd.DividePartitions();

            List<DVRPSolution> solutions = new List<DVRPSolution>();

            foreach (var partition in partitions)
            {
                DVRPPartialProblem partialProblem = new DVRPPartialProblem()
                {
                    ProblemInstance = problemInstance,
                    Partitions = partition
                };

                DVRPSolver solver = new DVRPSolver(partialProblem);
                var ret = solver.Solve();
                solutions.Add(ret);
            }

            return solutions.OrderBy(x => x.Cost).First();
        }
    }
}
