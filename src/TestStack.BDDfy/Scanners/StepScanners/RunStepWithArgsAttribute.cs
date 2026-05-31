using System;

namespace TestStack.BDDfy
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class RunStepWithArgsAttribute(params object[] inputArguments): Attribute
    {
        public string? StepTextTemplate { get; set; }

        public object[] InputArguments { get; } = inputArguments;
    }
}