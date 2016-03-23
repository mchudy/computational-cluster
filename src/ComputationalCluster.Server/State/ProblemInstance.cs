using ComputationalCluster.Common.Objects;

namespace ComputationalCluster.Server
{
    public class ProblemInstance
    {
        public int Id { get; set; }
        public byte[] Data { get; set; }
        public ulong? SolvingTimeout { get; set; }
        public string ProblemType { get; set; }
        public ProblemStatus Status { get; set; }
        public PartialProblemInstance[] PartialProblems { get; set; }
        public byte[] FinalSolution { get; set; }
    }

    public class PartialProblemInstance
    {
        public PartialProblem Problem { get; set; }
        public PartialProblemState State { get; set; }
        public int? NodeId { get; set; }
        public byte[] Solution { get; set; }
    }

    public enum PartialProblemState
    {
        New,
        ComputationOngoing,
        Computed
    }
}