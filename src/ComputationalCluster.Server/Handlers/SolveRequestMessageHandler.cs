﻿using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Networking;
using log4net;
using System.Collections.Generic;

namespace ComputationalCluster.Server.Handlers
{
    public class SolveRequestMessageHandler : IMessageHandler<SolveRequestMessage>
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(SolveRequestMessageHandler));
        private readonly IServerMessenger messenger;
        private readonly IServerContext context;

        public SolveRequestMessageHandler(IServerMessenger messenger, IServerContext context)
        {
            this.messenger = messenger;
            this.context = context;
        }

        public void HandleMessage(SolveRequestMessage message, ITcpClient client)
        {
            int id = context.GetNextProblemId();
            message.Id = (ulong?)id;
            context.BackupMessages.Enqueue(message);
            var problem = new ProblemInstance
            {
                Id = id,
                Data = message.Data,
                ProblemType = message.ProblemType,
                SolvingTimeout = message.SolvingTimeout,
                Status = ProblemStatus.New
            };
            context.Problems.Add(problem);
            SendResponse(client.GetStream(), id);
            logger.Info("Recieved SolveRequestMessage of type: " + message.ProblemType + " Timeout:" + message.SolvingTimeout);
        }

        private void SendResponse(INetworkStream stream, int id)
        {
            if (!context.IsPrimary) return;
            var response = new SolveRequestResponseMessage
            {
                Id = (ulong)id
            };
            List<Message> messages = new List<Message>
            {
                response,
                context.GetNoOperationMessage()
            };
            messenger.SendMessages(messages, stream);
        }
    }
}
