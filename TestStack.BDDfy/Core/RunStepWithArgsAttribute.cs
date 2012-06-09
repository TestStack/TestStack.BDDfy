using System;

namespace TestStack.BDDfy.Core
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class RunStepWithArgsAttribute : Attribute
    {
        private readonly object[] _inputArguments;

        public RunStepWithArgsAttribute(params object[] inputArguments)
        {
            _inputArguments = inputArguments;
        }

        public string StepTextTemplate { get; set; }

        public object[] InputArguments
        {
            get { return _inputArguments; }
        }
    }
}