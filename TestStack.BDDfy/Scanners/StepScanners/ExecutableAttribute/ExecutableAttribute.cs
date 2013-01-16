using System;
using TestStack.BDDfy.Core;

namespace TestStack.BDDfy.Scanners.StepScanners.ExecutableAttribute
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ExecutableAttribute : Attribute
    {
        public ExecutableAttribute(ExecutionOrder order, string stepTitle)
        {
            ExecutionOrder = order;
            StepTitle = stepTitle;
        }

        public ExecutionOrder ExecutionOrder { get; private set; }
        public bool Asserts { get; set; }
        public string StepTitle { get; set; }
        public int Order { get; set; }
    }
}