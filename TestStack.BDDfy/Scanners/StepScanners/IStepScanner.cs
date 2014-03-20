using System.Collections.Generic;
using System.Reflection;

namespace TestStack.BDDfy
{
    public interface IStepScanner
    {
        IEnumerable<Step> Scan(object testObject, MethodInfo candidateMethod);
    }
}