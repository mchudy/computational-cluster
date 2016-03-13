using ComputationalCluster.Common;
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Serialization;
using System;

namespace ComputationalCluster.TaskManager
{
    class Program
    {
        static void Main(string[] args)
        {
            StartTaskManager();
            Console.ReadLine();
        }

        private static void StartTaskManager()
        {
            var serializer = new MessageSerializer();
            var messenger = new Messenger(serializer);
            var message = new ErrorMessage
            {
                ErrorType = ErrorErrorType.ExceptionOccured
            };
            try
            {
                messenger.SendMessageAndClose(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
