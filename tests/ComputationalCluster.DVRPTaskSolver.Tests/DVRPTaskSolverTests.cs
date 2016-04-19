using ComputationalCluster.DVRPTaskSolver.Parsing;
using System.IO;
using Xunit;

namespace ComputationalCluster.DVRPTaskSolver.Tests
{
    public class DVRPTaskSolverTests
    {
        [Fact]
        public void ShouldNotThrowExceptionGivenNullDataInConstructor()
        {
            var solver = new DVRPTaskSolver(null);
        }

        [Fact]
        public void ShouldParseio2_5_plain_a_D()
        {
            DVRPParser parser = new DVRPParser();
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\io2_5_plain_a_D.vrp";
            var ret = parser.ParseFile(path);
            Assert.Equal(ret.Clients.Length,5);
            Assert.Equal(ret.Depots.Length, 1);
            Assert.Equal(ret.VehicleCapacity, 100);
            Assert.Equal(ret.VehiclesCount, 5);
        }
    }
}
