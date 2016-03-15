﻿using ComputationalCluster.Common;
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Serialization;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ComputationalCluster.Server
{
    public class Server
    {
        private readonly IMessageDispatcher messageDispatcher;
        private readonly IMessageSerializer serializer;
        //TODO: read from config
        private const int port = 9000;

        public Server(IMessageDispatcher messageDispatcher, IMessageSerializer serializer)
        {
            this.messageDispatcher = messageDispatcher;
            this.serializer = serializer;
        }

        public void Start()
        {
            TcpListener server = new TcpListener(IPAddress.Any, port);
            server.Start();
            Console.WriteLine($"Started listening on {server.LocalEndpoint}");
            while (true)
            {
                AcceptClient(server);
            }
        }

        private void AcceptClient(TcpListener server)
        {
            var client = server.AcceptTcpClient();
            Console.WriteLine($"New connection {client.Client.RemoteEndPoint}");
            try
            {
                using (var stream = client.GetStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        //TODO: not sure if message must end with ETB... probably should wait some timeout in case it doesn't
                        string messageString = ReadMessage(reader);

                        Console.WriteLine($"\nMessage from {client.Client.RemoteEndPoint}\n{messageString}\n");
                        Message message = serializer.Deserialize(messageString);
                        messageDispatcher.Dispatch(message, client);
                    }
                }
            }
            catch (IOException)
            {
                Console.WriteLine("Connection lost");
            }
            finally
            {
                client.Close();
            }
        }

        private string ReadMessage(StreamReader reader)
        {
            return ReadToChar(reader, Constants.ETB);
        }

        //TODO: Read by blocks to optimize?; change into extension method
        public static string ReadToChar(StreamReader sr, char splitCharacter)
        {
            char nextChar;
            StringBuilder line = new StringBuilder();
            while (sr.Peek() > 0)
            {
                nextChar = (char)sr.Read();
                if (nextChar == splitCharacter) return line.ToString();
                line.Append(nextChar);
            }
            return line.Length == 0 ? null : line.ToString();
        }
    }
}
