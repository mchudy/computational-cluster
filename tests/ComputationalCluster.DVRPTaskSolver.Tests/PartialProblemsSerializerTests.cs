using ComputationalCluster.DVRPTaskSolver.Algorithm;
using ComputationalCluster.DVRPTaskSolver.Problem;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ComputationalCluster.DVRPTaskSolver.Tests
{
    public class PartialProblemsSerializerTests
    {
        [Fact]
        public void ShouldSerialize()
        {
            var serialzier = new PartialProblemsSerializer();
            var data = serialzier.Serialize(new DVRPProblemInstance
            {
                Clients = new[] { new Client { X = 3, AvailableTime = 3 } },
                Depots = new[] { new Depot { X = 0, Y = 0 } }
            }, new List<Partition>[]
            {
                new List<Partition> {new Partition(2), new Partition(3)},
            });
            var json = Encoding.UTF8.GetString(data[0]);
        }
    }
}
