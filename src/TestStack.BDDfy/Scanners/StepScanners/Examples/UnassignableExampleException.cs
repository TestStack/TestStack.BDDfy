using System;

namespace TestStack.BDDfy
{
#if NET40
    [System.Serializable]
#else
    [System.Runtime.Serialization.Serializable]
#endif
    public class UnassignableExampleException : Exception
    {
        public UnassignableExampleException(string message, Exception inner, ExampleValue exampleValue) : base(message, inner)
        {
            ExampleValue = exampleValue;
        }
#if NET40

        protected UnassignableExampleException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }

#endif
        public ExampleValue ExampleValue { get; private set; }
    }
}