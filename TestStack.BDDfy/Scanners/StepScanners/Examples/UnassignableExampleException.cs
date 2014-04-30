using System;
using System.Runtime.Serialization;

namespace TestStack.BDDfy
{
    [Serializable]
    public class UnassignableExampleException : Exception
    {
        public UnassignableExampleException()
        {
        }

        public UnassignableExampleException(string message) : base(message)
        {
        }

        public UnassignableExampleException(string message, Exception inner) : base(message, inner)
        {
        }

        protected UnassignableExampleException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}