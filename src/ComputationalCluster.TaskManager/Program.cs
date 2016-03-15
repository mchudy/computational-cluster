using Autofac;
using Autofac.Core;
using ComputationalCluster.Common;
using System;

namespace ComputationalCluster.TaskManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<CommonModule>();
            builder.RegisterType<TaskManager>()
                .AsSelf()
                .SingleInstance();

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
