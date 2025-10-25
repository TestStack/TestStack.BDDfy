using System;

namespace TestStack.BDDfy
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ExecutableAttribute(ExecutionOrder order, string stepTitle): Attribute
    {
        public ExecutionOrder ExecutionOrder { get; private set; } = order;
        public bool Asserts { get; set; }
        public string StepTitle { get; set; } = stepTitle;
        public int Order { get; set; }
        public bool ShouldReport { get; set; } = true;
    }
}