using System;

namespace TestStack.BDDfy
{
    /// <summary>
    /// Allows examples to contain actions which can be performed
    /// </summary>
    public class ExampleAction
    {
        private readonly string _stepTitle;

        public ExampleAction(string stepTitle, Action action)
        {
            Action = action;
            _stepTitle = stepTitle;
        }

        public Action Action { get; private set; }

        public override string ToString()
        {
            return _stepTitle;
        }
    }
}