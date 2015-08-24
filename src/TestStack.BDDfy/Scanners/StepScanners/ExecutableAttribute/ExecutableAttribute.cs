using System;

namespace TestStack.BDDfy
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ExecutableAttribute : Attribute
    {
        public ExecutableAttribute(ExecutionOrder order, string stepTitle)
        {
            ExecutionOrder = order;
            StepTitle = stepTitle;
            ShouldReport = true;
        }

        public ExecutionOrder ExecutionOrder { get; private set; }
        public bool Asserts { get; set; }
        public string StepTitle { get; set; }
        public int Order { get; set; }
        public bool ShouldReport { get; set; }
    }
}