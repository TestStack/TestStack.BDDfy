using System;
using System.Runtime.Serialization;

namespace Bddify.Processors
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

        protected InconclusiveException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}