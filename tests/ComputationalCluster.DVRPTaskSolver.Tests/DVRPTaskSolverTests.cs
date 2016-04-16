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
    }
}
