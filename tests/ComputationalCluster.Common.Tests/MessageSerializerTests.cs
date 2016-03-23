using ComputationalCluster.Common.Exceptions;
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Serialization;
using System.IO;
using Xunit;

namespace ComputationalCluster.Common.Tests
{
    public class MessageSerializerTests
    {
        [Fact]
        public void Deserialize_ShouldReturnCorrectType()
        {
            var xml = File.ReadAllText("SampleMessages/Register.xml");
            var serializer = new MessageSerializer();

            var message = serializer.Deserialize(xml);

            Assert.Equal(typeof(RegisterMessage), message.GetType());
        }

        [Fact]
        public void Deserialize_ShouldAssignProperties()
        {
            var message = DeserializeSampleRegisterMessage();

            Assert.Equal(12u, message.Id);
            Assert.Equal(8, message.ParallelThreads);
            Assert.Equal(false, message.Deregister);
            Assert.Equal(ClientComponentType.TaskManager, message.Type.Type);
        }

        [Fact]
        public void Deserialize_ShouldAssignSolvableProblems()
        {
            var message = DeserializeSampleRegisterMessage();

            Assert.Equal(2, message.SolvableProblems.Length);
            Assert.Equal("TSP", message.SolvableProblems[0]);
            Assert.Equal("DVRP", message.SolvableProblems[1]);
        }

        [Fact]
        public void Deserialize_GivenMalformedXml_ShouldThrow()
        {
            string message = "aaa";
            var serializer = new MessageSerializer();
            Assert.Throws<CannotReadMessageException>(() => serializer.Deserialize(message));
        }

        private static RegisterMessage DeserializeSampleRegisterMessage()
        {
            var xml = File.ReadAllText("SampleMessages/Register.xml");
            var serializer = new MessageSerializer();

            var message = (RegisterMessage)serializer.Deserialize(xml);
            return message;
        }

    }
}
