using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Objects;
using log4net;
using System;
using System.IO;
using System.Text;

namespace ComputationalCluster.Client.Handlers
{
    public class SolutionsMessageHandler : IResponseHandler<SolutionMessage>
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(SolutionsMessageHandler));
        private readonly ClientContext context;

        public SolutionsMessageHandler(ClientContext context)
        {
            this.context = context;
        }

        public void HandleResponse(SolutionMessage response)
        {
            if (response.Solutions == null || response.Solutions.Length == 0)
            {
                logger.Warn("Got empty solution list");
            }
            else if (response.Solutions[0].Type == SolutionType.Ongoing)
            {
                logger.Debug("Computations still ongoing");
            }
            else if (response.Solutions[0].Type == SolutionType.Final)
            {
                context.CurrentProblemId = null;
                context.Stopwatch?.Stop();
                byte[] finalSolutionData = response.Solutions[0].Data;
                string solutionString = Encoding.UTF8.GetString(finalSolutionData);
                logger.Info($"Final solution received: \n{solutionString}");
                logger.Info($"Total computation time: {context.Stopwatch?.Elapsed}");
                WriteFile(solutionString);
            }
        }

        private void WriteFile(string solutionString)
        {
            int lastLine = solutionString.LastIndexOf(Environment.NewLine, StringComparison.Ordinal);
            if (lastLine > 0)
            {
                string pureSolution = solutionString.Remove(lastLine);
                logger.Info("Creating file with solution");
                File.WriteAllText(context.ProblemFileName + ".solution.txt", pureSolution);
            }
        }
    }
}

