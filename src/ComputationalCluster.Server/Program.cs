using Autofac;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Serialization;
using ComputationalCluster.Server.Configuration;
using System;

namespace ComputationalCluster.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = BuildContainer();
            var server = container.Resolve<Server>();

            //server.Start();

            var options = new ServerOptions();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                Console.WriteLine("Port: {0}", options.ListeningPort);
                Console.WriteLine("Timeout: {0}", options.Timeout);
                Console.WriteLine("Backup {0}", options.Backup);
                Console.WriteLine("MAddres: {0}", options.MasterServerAddress);
                Console.WriteLine("MPort: {0}", options.MasterServerPort);
            }
            else
            {
                Console.WriteLine(options.GetUsage());
            }
        }

        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MessageSerializer>()
                   .As<IMessageSerializer>();
            builder.RegisterType<ServerMessenger>()
                   .As<IServerMessenger>()
                   .InstancePerLifetimeScope();
            builder.RegisterType<ServerContext>()
                   .AsImplementedInterfaces()
                   .SingleInstance();
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                   .AsClosedTypesOf(typeof(IMessageHandler<>));
            builder.RegisterType<AutofacMessageDispatcher>()
                   .AsImplementedInterfaces();
            builder.RegisterType<Server>().AsSelf();
            builder.RegisterType<ServerConfiguration>()
                   .As<IServerConfiguration>()
                   .SingleInstance();
            builder.RegisterType<StatusChecker>()
                   .AsImplementedInterfaces()
                   .SingleInstance();

            return builder.Build();
        }
    }
}
