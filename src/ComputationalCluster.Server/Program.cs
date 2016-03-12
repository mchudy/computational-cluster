using Autofac;
using ComputationalCluster.Common;
using ComputationalCluster.Common.Serialization;

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
                   .AsImplementedInterfaces();
            builder.RegisterType<Server>().AsSelf();

            var container = builder.Build();
            var server = container.Resolve<Server>();
            server.Start();
        }
    }
}
