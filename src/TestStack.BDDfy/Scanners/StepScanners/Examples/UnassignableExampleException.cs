using System;

namespace TestStack.BDDfy
{
    public class UnassignableExampleException : Exception
    {
        public UnassignableExampleException(string message, Exception inner, ExampleValue exampleValue) : base(message, inner)
        {
            ExampleValue = exampleValue;
        }

        public ExampleValue ExampleValue { get; private set; }
    }
}