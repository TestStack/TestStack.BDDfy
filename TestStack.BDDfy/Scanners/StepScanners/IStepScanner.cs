using System.Collections.Generic;
using System.Reflection;
using TestStack.BDDfy.Core;

namespace TestStack.BDDfy.Scanners
{
    public interface IStepScanner
    {
        IEnumerable<ExecutionStep> Scan(object testObject, MethodInfo candidateMethod);
    }
}