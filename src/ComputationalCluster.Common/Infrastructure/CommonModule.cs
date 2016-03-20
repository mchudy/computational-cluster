using Autofac;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Networking.Factories;
using ComputationalCluster.Common.Serialization;

namespace ComputationalCluster.Common.Infrastructure
{
    public class CommonModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Configuration>()
                   .AsImplementedInterfaces()
                   .SingleInstance();
            builder.RegisterType<MessageSerializer>()
                   .AsImplementedInterfaces();
            builder.RegisterType<Messenger>()
                   .AsImplementedInterfaces();
            builder.RegisterType<TcpClientFactory>()
                   .AsImplementedInterfaces()
                   .SingleInstance();
            builder.RegisterType<MessageStreamFactory>()
                   .AsImplementedInterfaces();
        }
    }
}
