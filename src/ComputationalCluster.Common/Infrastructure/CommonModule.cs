using Autofac;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Networking.Factories;
using ComputationalCluster.Common.Serialization;
using Module = Autofac.Module;

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
                   .AsImplementedInterfaces()
                   .InstancePerDependency();
            builder.RegisterType<TcpClientFactory>()
                   .AsImplementedInterfaces()
                   .SingleInstance();
            builder.RegisterType<MessageStreamFactory>()
                   .AsImplementedInterfaces();
            builder.RegisterType<AutofacResponseDispatcher>()
                   .AsImplementedInterfaces();
            builder.RegisterType<TaskSolverProvider>()
                   .AsImplementedInterfaces()
                   .SingleInstance();
        }
    }
}
