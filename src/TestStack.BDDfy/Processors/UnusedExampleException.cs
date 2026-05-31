using System;

namespace TestStack.BDDfy.Processors
{
    [Serializable]
    public class UnusedExampleException(ExampleValue unusedValue): Exception(
        string.Format("Example Column '{0}' is unused, all examples should be consumed by the test (have you misspelt a field or property?)\r\n\r\n"
        + "If this is not the case, raise an issue at https://github.com/TestStack/TestStack.BDDfy/issues.", unusedValue.Header))
    {
    }
}