using System;
using System.Runtime.Serialization;

namespace Bddify.Scanners.StepScanners
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

#if !SILVERLIGHT
        protected StepTitleException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}