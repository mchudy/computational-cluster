using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using UCCTaskSolver;

namespace ComputationalCluster.Common
{
    public class TaskSolverProvider : ITaskSolverProvider
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(TaskSolverProvider));

        private readonly IDictionary<string, Func<byte[], TaskSolver>> taskSolvers
            = new Dictionary<string, Func<byte[], TaskSolver>>();

        public TaskSolverProvider()
        {
            LoadTaskSolvers();
        }

        public TaskSolver CreateTaskSolverInstance(string problemType, byte[] problemData)
        {
            Func<byte[], TaskSolver> factory;
            if (taskSolvers.TryGetValue(problemType, out factory))
            {
                return factory(problemData);
            }
            return null;
        }

        [SuppressMessage("ReSharper", "RedundantCast")]
        private void LoadTaskSolvers()
        {
            var taskSolverTypes = new List<Type>();
            if (!Directory.Exists(Constants.TaskSolverDirectory))
            {
                logger.Warn("Task solvers directory not found");
                return;
            }
            var files = Directory.GetFiles(Constants.TaskSolverDirectory, "*.dll")
                .Select(Path.GetFullPath);
            foreach (var file in files)
            {
                var assembly = Assembly.LoadFile(file);
                taskSolverTypes.AddRange(assembly.GetTypes()
                    .Where(t => !t.IsAbstract && !t.IsInterface && t.IsSubclassOf(typeof(TaskSolver))));
            }
            foreach (var type in taskSolverTypes)
            {
                // Assumes that TaskSolver constuctor will not throw exception on null parameter
                // Unfortunately there doesn't seem to be another way to check the value of the Name property
                // with accordance to the provided library
                TaskSolver instance = (TaskSolver)Activator.CreateInstance(type, (byte[])null);
                taskSolvers[instance.Name] = problemData => (TaskSolver)Activator.CreateInstance(type, problemData);
                logger.Info($"Registered task solver for {instance.Name} problems");
            }
        }
    }
}
