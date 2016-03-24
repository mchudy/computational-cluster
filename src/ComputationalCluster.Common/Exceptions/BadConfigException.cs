using System;

namespace ComputationalCluster.Common
{
    public class BadConfigException : Exception
    {
        public BadConfigException(string message)
            : base(message)
        {
        }
    }
}