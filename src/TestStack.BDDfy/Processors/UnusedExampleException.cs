using System;
using System.Runtime.Serialization;

namespace TestStack.BDDfy.Processors
{
#if NET40 
    [System.Runtime.Serialization.Serializable]
#else
    [System.Serializable]
#endif
    public class UnusedExampleException : Exception
    {
        public UnusedExampleException(ExampleValue unusedValue) :
            base(string.Format("Example Column '{0}' is unused, all examples should be consumed by the test (have you misspelt a field or property?)\r\n\r\n"
            + "If this is not the case, raise an issue at https://github.com/TestStack/TestStack.BDDfy/issues.", unusedValue.Header))
        { }
#if NET40

        protected UnusedExampleException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}