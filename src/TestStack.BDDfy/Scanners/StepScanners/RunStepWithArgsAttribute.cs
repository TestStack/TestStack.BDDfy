using System;

namespace TestStack.BDDfy
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class RunStepWithArgsAttribute(params object[] inputArguments): Attribute
    {
        private readonly object[] _inputArguments = inputArguments;

        public string StepTextTemplate { get; set; }

        public object[] InputArguments
        {
            get { return _inputArguments; }
        }
    }
}