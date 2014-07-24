using System;

namespace TestStack.BDDfy.Configuration
{
    public class DefaultStepExecutor : IStepExecutor
    {
        public object Execute(Func<object> step)
        {
            return step();
        }
    }
}