using Autofac;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Networking;
using ComputationalCluster.Common.Serialization;

namespace ComputationalCluster.Common.Infrastructure
{
    public class CommonModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Configuration>()
                   .As<IConfiguration>()
                   .SingleInstance();
            builder.RegisterType<MessageSerializer>()
                   .As<IMessageSerializer>();
            builder.RegisterType<Messenger>()
                   .As<IMessenger>();
            builder.RegisterType<TcpConnectionFactory>()
                   .As<ITcpConnectionFactory>()
                   .SingleInstance();
        }
    }
}
