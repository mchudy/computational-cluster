﻿using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Networking;
using System.Linq;

namespace ComputationalCluster.Server.Handlers
{
    public class PartialProblemMessageHandler : IMessageHandler<PartialProblemsMessage>
    {
        private readonly IServerContext context;

        public PartialProblemMessageHandler(IServerContext context)
        {
            this.context = context;
        }

        public void HandleMessage(PartialProblemsMessage message, ITcpClient client)
        {
            var problem = context.Problems.FirstOrDefault(p => p.Id == (int)message.Id);
            if (problem != null)
            {
                problem.Status = ProblemStatus.Divided;
                problem.PartialProblems = message.PartialProblems;
            }
        }
    }
}
