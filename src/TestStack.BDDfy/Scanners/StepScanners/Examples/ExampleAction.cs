using System;

namespace TestStack.BDDfy
{
    /// <summary>
    /// Allows examples to contain actions which can be performed
    /// </summary>
    public class ExampleAction(string stepTitle, Action action)
    {
        private readonly string _stepTitle = stepTitle;

        public Action Action { get; private set; } = action;

        public override string ToString()
        {
            return _stepTitle;
        }
    }
}