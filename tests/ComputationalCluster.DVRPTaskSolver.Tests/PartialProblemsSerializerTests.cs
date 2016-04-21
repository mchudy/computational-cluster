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

        private string json = @"{
                ""ProblemInstance"": {
                    ""Clients"" : [
                        {
                            ""AvailableTime"" : 10,
                            ""UnloadTime"" : 20,
                            ""X"" : 4,
                            ""Y"": 3,
                        }
                    ],
                    ""Depots"" : [
                        {
                            ""X"" : 0,
                            ""Y"": 1 ,
                            ""StartTime"" : 30,
                            ""EndTime"" : 50
                        }
                    ]
                },
                ""Partitions"": [
                    {
                        ""truckClients"": [
                            [1,2],
                            [3]
                        ]
                    }
                ]
            }";

        [Fact]
        public void ShouldDeserializePartialInstance()
        {
            var serializer = new PartialProblemsSerializer();
            var problem = serializer.Deserialize(Encoding.UTF8.GetBytes(json));

            var client = problem.ProblemInstance.Clients[0];
            Assert.Equal(10, client.AvailableTime);
            Assert.Equal(20, client.UnloadTime);
            Assert.Equal(4, client.X);
            Assert.Equal(3, client.Y);

            var depot = problem.ProblemInstance.Depots[0];
            Assert.Equal(0, depot.X);
            Assert.Equal(1, depot.Y);
            Assert.Equal(30, depot.StartTime);
            Assert.Equal(50, depot.EndTime);
        }

        [Fact]
        public void ShouldDeserializePartitions()
        {
            var serializer = new PartialProblemsSerializer();
            var problem = serializer.Deserialize(Encoding.UTF8.GetBytes(json));

            Assert.Equal(2, problem.Partitions[0].truckClients.Length);

            Assert.Equal(1, problem.Partitions[0].truckClients[0][0]);
            Assert.Equal(2, problem.Partitions[0].truckClients[0][1]);
            Assert.Equal(3, problem.Partitions[0].truckClients[1][0]);
        }
    }
}
