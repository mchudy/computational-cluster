﻿using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using System.Net.Sockets;
using ComputationalCluster.Common.Networking;

namespace ComputationalCluster.Server.Handlers
{
    public class SolutionMessageHandler : IMessageHandler<SolutionMessage>
    {
        public void HandleMessage(SolutionMessage message, ITcpConnection connection)
        {


        }
    }
}
