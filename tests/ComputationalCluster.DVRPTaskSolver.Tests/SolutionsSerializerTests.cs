using ComputationalCluster.DVRPTaskSolver.Problem;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ComputationalCluster.DVRPTaskSolver.Tests
{
    public class SolutionsSerializerTests
    {
        [Fact]
        public void ShouldSerializeSolution()
        {
            var serializer = new SolutionsSerializer();
            var json = serializer.Serialize(new DVRPSolution()
            {
                Cost = 250,
                Routes = new[]
                {
                    new List<int> {1, 2},
                    new List<int> { 3, 4},
                }
            });
        }

        [Fact]
        public void ShouldDeserializeSolution()
        {
            var json = @"{
                ""Cost"": 250,
                ""Routes"": [
                    [1, 2],
                    [3, 4]
                ]    
            }";
            var serializer = new SolutionsSerializer();
            var solution = serializer.Deserialize(new[] { Encoding.UTF8.GetBytes(json) });

            Assert.Equal(1, solution.Length);
            Assert.Equal(250, solution[0].Cost);
            Assert.Equal(2, solution[0].Routes.Length);

            Assert.Equal(1, solution[0].Routes[0][0]);
            Assert.Equal(2, solution[0].Routes[0][1]);

            Assert.Equal(3, solution[0].Routes[1][0]);
            Assert.Equal(4, solution[0].Routes[1][1]);

        }
    }
}
