using System;
using System.Runtime.Serialization;

namespace TestStack.BDDfy
{
#if NET40 
    [System.Runtime.Serialization.Serializable]
#else
    [System.Serializable]
#endif
    public class UnassignableExampleException : Exception
    {
        public UnassignableExampleException(string message, Exception inner, ExampleValue exampleValue) : base(message, inner)
        {
            ExampleValue = exampleValue;
        }
#if NET40

        protected UnassignableExampleException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

#endif
        public ExampleValue ExampleValue { get; private set; }
    }
}