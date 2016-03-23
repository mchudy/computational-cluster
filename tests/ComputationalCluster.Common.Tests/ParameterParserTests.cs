using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ComputationalCluster.Common.Tests
{
    public class ParameterParserTests
    {
        private const string correctParameters = "--port 1234 --address 123.123.123.123";
        private const int correctPort = 1234;
        private const string correctAddress = "123.123.123.123";

        [Fact]
        public void CorrectInputTest_ShouldBeCorrect()
        {
            CommonOptions options = new CommonOptions();
            CommonParameterParser.ParseParameters(correctParameters.Split(' '), ref options);
            Assert.Equal(correctPort, options.ServerPort);
            Assert.Equal(correctAddress, options.ServerAddress);
        }

        [Fact]
        public void EmptyInputTest_ShouldBeCorrect()
        {
            CommonOptions options = new CommonOptions();
            CommonParameterParser.ParseParameters(new string[] { "" }, ref options);
            Assert.Equal(0, options.ServerPort);
            Assert.Equal(null, options.ServerAddress);
        }
    }
}
