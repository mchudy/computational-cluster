using Autofac;
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
            var taskManager = container.Resolve<TaskManager>();
            taskManager.Start();

            Console.ReadLine();
        }
    }
}
