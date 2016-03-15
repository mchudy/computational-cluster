using Autofac;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Serialization;
using ComputationalCluster.Server.Configuration;

namespace ComputationalCluster.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                   .AsClosedTypesOf(typeof(IMessageHandler<>));
            builder.RegisterType<AutofacMessageDispatcher>()
                   .AsImplementedInterfaces();
            builder.RegisterType<MessageSerializer>()
                   .AsImplementedInterfaces()
                   .SingleInstance();
            builder.RegisterType<Server>().AsSelf();
            builder.RegisterType<ServerConfiguration>()
                   .As<IServerConfiguration>()
                   .SingleInstance();

            var container = builder.Build();
            var server = container.Resolve<Server>();
            server.Start();
        }
    }
}
