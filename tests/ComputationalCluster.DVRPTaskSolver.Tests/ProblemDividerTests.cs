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
    public class ProblemDividerTests
    {
        private DVRPProblemInstance problemInstance;
        private int threadCount;

        public ProblemDividerTests()
        {
            problemInstance = new DVRPProblemInstance
            {
                Depots = new Depot[] { new Depot(20, 20, 0, 300) },
                Clients = new Client[] { new Client(20, 20, 400, 20, 30), new Client(-20, -20, 400, 20, 40),
                                         new Client(-20, 20, 400, 20, 30) },
                VehicleCapacity = 100,
                VehiclesCount = 3
            };

            threadCount = 3;
        }

        [Fact]
        public void ProblemDivider_ShouldDivideProblem()
        {
            ProblemDivider pd = new ProblemDivider(problemInstance, threadCount);
            var ret = pd.DividePartitions();
            Assert.NotNull(ret);
            Assert.Equal(ret.Length, threadCount);
        }
    }
}
