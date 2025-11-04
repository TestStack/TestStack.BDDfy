using System;

namespace TestStack.BDDfy
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
    }
}