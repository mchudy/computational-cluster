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

        //[Fact]
        //public void ShouldParseio2_5_plain_a_D()
        //{
        //    DVRPParser parser = new DVRPParser();
        //    string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\io2_11_plain_a_D.vrp";
        //    var ret = parser.ParseFile(path);
        //    Assert.Equal(ret.Clients.Length,11);
        //    Assert.Equal(ret.Depots.Length, 1);
        //    Assert.Equal(ret.VehicleCapacity, 100);
        //    Assert.Equal(ret.VehiclesCount, 11);
        //    Assert.Equal(ret.Clients[0].AvailableTime, 230);
        //    Assert.Equal(ret.Clients[0].DemandSize, 15);
        //    Assert.Equal(ret.Clients[0].UnloadTime, 20);
        //    Assert.Equal(ret.Clients[0].X, 31);
        //    Assert.Equal(ret.Clients[0].Y, 1);
        //    Assert.Equal(ret.Depots[0].X, 0);
        //    Assert.Equal(ret.Depots[0].Y, 0);
        //    Assert.Equal(ret.Depots[0].StartHour,0 );
        //    Assert.Equal(ret.Depots[0].EndHour, 620 );
        //}
    }
}
