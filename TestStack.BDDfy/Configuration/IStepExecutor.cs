using System;

namespace TestStack.BDDfy.Configuration
{
    public interface IStepExecutor
    {
        object Execute(Func<object> step);
    }
}