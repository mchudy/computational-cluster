using Autofac;
using ComputationalCluster.Common;
using System;

namespace ComputationalCluster.Node
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<CommonModule>();
            builder.RegisterType<ComputationalNode>()
                   .AsSelf()
                   .SingleInstance();

            var container = builder.Build();
            var taskManager = container.Resolve<ComputationalNode>();
            taskManager.Start();

            Console.ReadLine();
        }
    }
}
