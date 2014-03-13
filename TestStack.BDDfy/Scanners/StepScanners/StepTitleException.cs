using System;
using System.Runtime.Serialization;

namespace TestStack.BDDfy.Scanners
{
    public class StepTitleException : Exception
    {
        public StepTitleException()
        {
        }

        public StepTitleException(string message) : base(message)
        {
        }

        public StepTitleException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StepTitleException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}