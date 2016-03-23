using Autofac;
using Autofac.Core;
using ComputationalCluster.Common;
using ComputationalCluster.Common.Messaging;
using System;

namespace ComputationalCluster.TaskManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof(Constants).Assembly);
            builder.RegisterType<TaskManager>()
                .AsSelf()
                .SingleInstance();
            builder.RegisterType<TaskManagerContext>().AsSelf().SingleInstance();
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                   .AsClosedTypesOf(typeof(IResponseHandler<>))
                   .InstancePerDependency();

            var container = builder.Build();

            try
            {
                var taskManager = container.Resolve<TaskManager>();
                taskManager.Start();
            }
            catch (DependencyResolutionException e)
            {
                Console.Error.WriteLine(e.InnerException.Message);
            }
            Console.ReadLine();
        }
    }
}
