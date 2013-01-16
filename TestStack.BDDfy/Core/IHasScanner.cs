using System;

namespace TestStack.BDDfy.Core
{
    public interface IHasScanner
    {
        IScanner GetScanner(string scenarioTitle, Type explicitStoryType = null);
        object TestObject { get; }
    }
}
