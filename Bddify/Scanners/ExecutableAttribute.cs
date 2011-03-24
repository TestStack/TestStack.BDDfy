using System;

namespace Bddify.Scanners
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ExecutableAttribute : Attribute
    {
        public ExecutableAttribute(int order)
        {
            Order = order;
        }

        public int Order { get; private set; }
        public bool Asserts { get; set; }
        public string StepText { get; set; }
    }
}