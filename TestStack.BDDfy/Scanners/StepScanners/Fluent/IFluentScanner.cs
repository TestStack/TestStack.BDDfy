using System;

namespace TestStack.BDDfy
{
    public interface IFluentScanner
    {
        IScanner GetScanner(string scenarioTitle, Type explicitStoryType, IExamples examples);
    }
}
