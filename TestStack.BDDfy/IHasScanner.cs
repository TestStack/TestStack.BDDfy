using System;

namespace TestStack.BDDfy
{
    public interface IHasScanner
    {
        IScanner GetScanner(string scenarioTitle, Type explicitStoryType = null);
        object TestObject { get; }
    }
}
