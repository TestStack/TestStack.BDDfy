using System;
using Bddify.Core;

namespace Bddify.Scanners
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ExecutableAttribute : Attribute
    {
        public ExecutableAttribute(ExecutionOrder order)
        {
            ExecutionOrder = order;
        }

        public ExecutionOrder ExecutionOrder { get; private set; }
        public bool Asserts { get; set; }
        public string StepText { get; set; }
    }
}