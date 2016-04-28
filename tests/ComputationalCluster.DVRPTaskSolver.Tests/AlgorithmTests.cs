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
        private DVRPProblemInstance problemInstance6;
        private DVRPProblemInstance problemInstance7;
        private int threadCount;

        public AlgorithmTests()
        {
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

            problemInstance6 = new DVRPProblemInstance
            {
                Depots = new Depot[] { new Depot(0, 0, 0, 520) },
                Clients = new Client[] { new Client(-87, 49, 512, 20, 38), new Client(-43, 50, 198, 20, 13),
                                         new Client(-7, -16, 146, 20, 33), new Client(88, -98, 476, 20, 13),
                                         new Client(53, -19, 1, 20, 30), new Client(-23, 85, 64, 20, 21)},
                VehicleCapacity = 100,
                VehiclesCount = 6
            };

            problemInstance7 = new DVRPProblemInstance
            {
                Depots = new Depot[] { new Depot(0, 0, 0, 540) },
                Clients = new Client[] { new Client(-29, -95, 180, 20, 25), new Client(31, 48, 125, 20, 9),
                                         new Client(-29, 25, 458, 20, 26), new Client(-93, 35, 190, 20, 26),
                                         new Client(-84, -30, 493, 20, 23), new Client(-50, 42, 448, 20, 26),
                                         new Client(-66, 55, 380, 20, 10)},
                VehicleCapacity = 100,
                VehiclesCount = 7
            };

            threadCount = 4;
        }

        [Fact]
        void Solve_ShouldSolve4Client1Depot_CheckRoute()
        {
            var ret = Solve(problemInstance4);

            SolutionsSerializer ss = new SolutionsSerializer();
            string solutionString = Encoding.UTF8.GetString(ss.Serialize(ret));

            Assert.NotNull(ret);
            Assert.Contains("4,1", solutionString);
            Assert.Contains("2,3", solutionString);
        }

        [Fact]
        void Solve_ShouldSolve5Client1Depot_CheckRoute()
        {
            var ret = Solve(problemInstance5);

            SolutionsSerializer ss = new SolutionsSerializer();
            string solutionString = Encoding.UTF8.GetString(ss.Serialize(ret));

            Assert.NotNull(ret);
            Assert.Contains("3,5,1", solutionString);
            Assert.Contains("4,2", solutionString);
        }

        [Fact]
        void Solve_ShouldSolve5Client1Depot_CheckCost()
        {
            var ret = Solve(problemInstance5);

            SolutionsSerializer ss = new SolutionsSerializer();
            string solutionString = Encoding.UTF8.GetString(ss.Serialize(ret));

            Assert.NotNull(ret);
            Assert.Equal((int)ret.Cost, 446);
        }

        [Fact]
        void Solve_ShouldSolve6Client1Depot_CheckCost()
        {
            var ret = Solve(problemInstance6);

            SolutionsSerializer ss = new SolutionsSerializer();
            string solutionString = Encoding.UTF8.GetString(ss.Serialize(ret));

            Assert.NotNull(ret);
            Assert.Equal((int)ret.Cost, 557);
        }

        [Fact]
        void Solve_ShouldSolve6Client1Depot_CheckRoute()
        {
            var ret = Solve(problemInstance6);

            SolutionsSerializer ss = new SolutionsSerializer();
            string solutionString = Encoding.UTF8.GetString(ss.Serialize(ret));

            Assert.NotNull(ret);
            Assert.Contains("6,1,2", solutionString);
            Assert.Contains("5,4,3", solutionString);
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
