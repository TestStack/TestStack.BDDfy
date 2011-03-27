using System;

namespace Bddify.Core
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class RunStepWithArgsAttribute : Attribute
    {
        private readonly object[] _inputArguments;

        public RunStepWithArgsAttribute(params object[] inputArguments)
        {
            _inputArguments = inputArguments;
        }

        public object[] InputArguments
        {
            get { return _inputArguments; }
        }
    }
}