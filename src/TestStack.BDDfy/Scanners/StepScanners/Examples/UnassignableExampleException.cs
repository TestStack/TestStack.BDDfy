using System;

namespace TestStack.BDDfy
{
    [Serializable]
    public class UnassignableExampleException(string message, Exception inner, ExampleValue exampleValue): Exception(message, inner)
    {
        public ExampleValue ExampleValue { get; private set; } = exampleValue;
    }
}