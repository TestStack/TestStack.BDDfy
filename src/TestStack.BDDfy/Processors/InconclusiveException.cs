using System;
using System.Runtime.Serialization;

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
#if NET40

        protected InconclusiveException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }
}