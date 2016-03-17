using Autofac;
using ComputationalCluster.Common;
using ComputationalCluster.Common.Infrastructure;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Serialization;
using ComputationalCluster.Server.Configuration;

namespace ComputationalCluster.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = BuildContainer();

            var server = container.Resolve<Server>();
            server.Start();
        }

        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<LoggingModule>();
            builder.RegisterType<MessageSerializer>()
                   .As<IMessageSerializer>();
            builder.RegisterType<ServerMessenger>()
                   .As<IServerMessenger>()
                   .InstancePerLifetimeScope();
            builder.RegisterType<ServerContext>()
                   .AsSelf()
                   .SingleInstance();
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                   .AsClosedTypesOf(typeof(IMessageHandler<>));
            builder.RegisterType<AutofacMessageDispatcher>()
                   .AsImplementedInterfaces();
            builder.RegisterType<Server>().AsSelf();
            builder.RegisterType<ServerConfiguration>()
                   .As<IServerConfiguration>()
                   .SingleInstance();

            return builder.Build();
        }
    }
}
