﻿using System;
using System.Net;
using System.Net.Sockets;

namespace ComputationalCluster.Common.Networking
{
    public interface ITcpConnection : IDisposable
    {
        IPEndPoint EndPoint { get; }
        void Connect(string address, int port);
        NetworkStream GetStream();
    }
}