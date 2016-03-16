using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputationalCluster.Common;
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;

namespace ComputationalCluster.Client
{
    class Client
    {
        private readonly IMessenger messenger;
        private readonly IConfiguration configuration;
        private ulong id;

        public Client(IMessenger messenger, IConfiguration configuration)
        {
            this.configuration = configuration;
            this.messenger = messenger;
        }

        public void Start()
        {
            var message = new SolveRequestMessage()
            {
                ProblemType = "DVRP",
                SolvingTimeout = 99999,
                SolvingTimeoutSpecified = true,
                Id = 123,
                IdSpecified = false,
                Data = new byte[] {1, 2, 3}              
            };
            try
            {
                IList<Message> responses = messenger.SendMessage(message);
                var responseMessage = responses[0] as SolveRequestResponseMessage;
                if (responseMessage != null)
                {
                    var response = responseMessage;
                    id = response.Id;
                    Console.WriteLine($"SolveRequestResponse with id {id}");
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
