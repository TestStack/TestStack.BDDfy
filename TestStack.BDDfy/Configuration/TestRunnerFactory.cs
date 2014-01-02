using System;
using TestStack.BDDfy.Core;

namespace TestStack.BDDfy.Configuration
{
    public class TestRunnerFactory : ProcessorFactory
    {
        internal TestRunnerFactory(Func<IProcessor> factory) : base(factory)
        {
            StopExecutionOnFailingThen = false;
        }

        internal TestRunnerFactory(Func<IProcessor> factory, bool active) : base(factory, active)
        {
            StopExecutionOnFailingThen = false;
        }

        /// <summary>
        /// Set to true if you want the execution pipleline to stop when a Then step fails. Defaulted to false
        /// </summary>
        public bool StopExecutionOnFailingThen { get; set; }
    }
}
