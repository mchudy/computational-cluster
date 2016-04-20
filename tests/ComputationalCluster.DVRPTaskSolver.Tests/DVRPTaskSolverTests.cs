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
        public void Parse_ShouldParseVehicles_io2_4_plain_a_D()
        {
            DVRPParser parser = new DVRPParser();
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\testFiles\\io2_4_plain_a_D.vrp";
            FileStream fStream = new FileStream(path, FileMode.Open);
            byte[] bytes = new byte[fStream.Length];
            fStream.Read(bytes, 0, bytes.Length);

            var ret = parser.ParseFile(bytes);
            Assert.Equal(ret.VehicleCapacity, 100);
            Assert.Equal(ret.VehiclesCount, 4);

            fStream.Dispose();
        }
        [Fact]
        public void Parse_ShouldParseClients_io2_4_plain_a_D()
        {
            DVRPParser parser = new DVRPParser();
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\testFiles\\io2_4_plain_a_D.vrp";
            FileStream fStream = new FileStream(path, FileMode.Open);
            byte[] bytes = new byte[fStream.Length];
            fStream.Read(bytes, 0, bytes.Length);

            var ret = parser.ParseFile(bytes);
            Assert.Equal(ret.Clients.Length, 4);

            Assert.Equal(ret.Clients[0].AvailableTime, 339);
            Assert.Equal(ret.Clients[0].DemandSize, 11);
            Assert.Equal(ret.Clients[0].UnloadTime, 20);
            Assert.Equal(ret.Clients[0].X, -1);
            Assert.Equal(ret.Clients[0].Y, 36);

            Assert.Equal(ret.Clients[1].AvailableTime, 87);
            Assert.Equal(ret.Clients[1].DemandSize, 15);
            Assert.Equal(ret.Clients[1].UnloadTime, 20);
            Assert.Equal(ret.Clients[1].X, -29);
            Assert.Equal(ret.Clients[1].Y, -99);

            Assert.Equal(ret.Clients[2].AvailableTime, 215);
            Assert.Equal(ret.Clients[2].DemandSize, 40);
            Assert.Equal(ret.Clients[2].UnloadTime, 20);
            Assert.Equal(ret.Clients[2].X, -5);
            Assert.Equal(ret.Clients[2].Y, 14);

            Assert.Equal(ret.Clients[3].AvailableTime, 71);
            Assert.Equal(ret.Clients[3].DemandSize, 27);
            Assert.Equal(ret.Clients[3].UnloadTime, 20);
            Assert.Equal(ret.Clients[3].X, -79);
            Assert.Equal(ret.Clients[3].Y, -91);

            fStream.Dispose();
        }

        [Fact]
        public void Parse_ShouldParseDepots_io2_4_plain_a_D()
        {
            DVRPParser parser = new DVRPParser();
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\testFiles\\io2_4_plain_a_D.vrp";
            FileStream fStream = new FileStream(path, FileMode.Open);
            byte[] bytes = new byte[fStream.Length];
            fStream.Read(bytes, 0, bytes.Length);

            var ret = parser.ParseFile(bytes);
            Assert.Equal(ret.Depots.Length, 1);

            Assert.Equal(ret.Depots[0].StartTime, 0);
            Assert.Equal(ret.Depots[0].EndTime, 480);
            Assert.Equal(ret.Depots[0].X, 0);
            Assert.Equal(ret.Depots[0].Y, 0);

            fStream.Dispose();
        }

        [Fact]
        public void Parse_ShouldParseVehicles_io2_5_plain_a_D()
        {
            DVRPParser parser = new DVRPParser();
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\testFiles\\io2_5_plain_a_D.vrp";
            FileStream fStream = new FileStream(path, FileMode.Open);
            byte[] bytes = new byte[fStream.Length];
            fStream.Read(bytes, 0, bytes.Length);

            var ret = parser.ParseFile(bytes);
            Assert.Equal(ret.VehicleCapacity, 100);
            Assert.Equal(ret.VehiclesCount, 5);

            fStream.Dispose();
        }
        [Fact]
        public void Parse_ShouldParseClients_io2_5_plain_a_D()
        {
            DVRPParser parser = new DVRPParser();
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\testFiles\\io2_5_plain_a_D.vrp";
            FileStream fStream = new FileStream(path, FileMode.Open);
            byte[] bytes = new byte[fStream.Length];
            fStream.Read(bytes, 0, bytes.Length);

            var ret = parser.ParseFile(bytes);
            Assert.Equal(ret.Clients.Length, 5);

            Assert.Equal(ret.Clients[0].AvailableTime, 372);
            Assert.Equal(ret.Clients[0].DemandSize, 4);
            Assert.Equal(ret.Clients[0].UnloadTime, 20);
            Assert.Equal(ret.Clients[0].X, 50);
            Assert.Equal(ret.Clients[0].Y, -41);

            Assert.Equal(ret.Clients[1].AvailableTime, 219);
            Assert.Equal(ret.Clients[1].DemandSize, 33);
            Assert.Equal(ret.Clients[1].UnloadTime, 20);
            Assert.Equal(ret.Clients[1].X, 48);
            Assert.Equal(ret.Clients[1].Y, -29);

            Assert.Equal(ret.Clients[2].AvailableTime, 313);
            Assert.Equal(ret.Clients[2].DemandSize, 39);
            Assert.Equal(ret.Clients[2].UnloadTime, 20);
            Assert.Equal(ret.Clients[2].X, -1);
            Assert.Equal(ret.Clients[2].Y, -82);

            Assert.Equal(ret.Clients[3].AvailableTime, 133);
            Assert.Equal(ret.Clients[3].DemandSize, 30);
            Assert.Equal(ret.Clients[3].UnloadTime, 20);
            Assert.Equal(ret.Clients[3].X, -66);
            Assert.Equal(ret.Clients[3].Y, 55);

            Assert.Equal(ret.Clients[4].AvailableTime, 135);
            Assert.Equal(ret.Clients[4].DemandSize, 34);
            Assert.Equal(ret.Clients[4].UnloadTime, 20);
            Assert.Equal(ret.Clients[4].X, 57);
            Assert.Equal(ret.Clients[4].Y, 39);

            fStream.Dispose();
        }

        [Fact]
        public void Parse_ShouldParseDepots_io2_5_plain_a_D()
        {
            DVRPParser parser = new DVRPParser();
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\testFiles\\io2_5_plain_a_D.vrp";
            FileStream fStream = new FileStream(path, FileMode.Open);
            byte[] bytes = new byte[fStream.Length];
            fStream.Read(bytes, 0, bytes.Length);

            var ret = parser.ParseFile(bytes);
            Assert.Equal(ret.Depots.Length, 1);

            Assert.Equal(ret.Depots[0].StartTime, 0);
            Assert.Equal(ret.Depots[0].EndTime, 500);
            Assert.Equal(ret.Depots[0].X, 0);
            Assert.Equal(ret.Depots[0].Y, 0);

            fStream.Dispose();
        }
    }
}
