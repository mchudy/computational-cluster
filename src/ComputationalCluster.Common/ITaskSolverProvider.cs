using UCCTaskSolver;

namespace ComputationalCluster.Common
{
    public interface ITaskSolverProvider
    {
        TaskSolver CreateTaskSolverInstance(string problemType, byte[] problemData);
    }
}