using System;
using System.Runtime.Serialization;

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
 #if NET40

       protected StepTitleException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}