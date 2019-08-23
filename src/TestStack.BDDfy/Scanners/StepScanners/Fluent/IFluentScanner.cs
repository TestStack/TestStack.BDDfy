using System;
using System.Reflection;

namespace TestStack.BDDfy
{
    public interface IFluentScanner
    {
        IScanner GetScanner(string scenarioTitle, Type explicitStoryType);
        void SetCreateTitle(Func<string, bool, MethodInfo, StepArgument[], string, StepTitle> customCreateTitle);
    }
}
