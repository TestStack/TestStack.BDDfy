using System;

namespace TestStack.BDDfy.Processors
{
    public class InconclusiveException : Exception
    {
        public InconclusiveException()
        {
        }

        public InconclusiveException(string message) : base(message)
        {
        }

        public InconclusiveException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}