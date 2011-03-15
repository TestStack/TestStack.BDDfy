using System;

namespace Bddify
{
    public class WithArgsAttribute : Attribute
    {
        private readonly object[] _inputArguments;

        public WithArgsAttribute(params object[] inputArguments)
        {
            _inputArguments = inputArguments;
        }

        public object[] InputArguments
        {
            get { return _inputArguments; }
        }
    }
}