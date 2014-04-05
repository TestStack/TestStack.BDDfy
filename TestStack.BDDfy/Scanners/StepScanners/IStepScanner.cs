using System.Collections.Generic;
using System.Reflection;

namespace TestStack.BDDfy
{
    public interface IStepScanner
    {
        IEnumerable<Step> Scan(object testObject, MethodInfo method);
        IEnumerable<Step> Scan(object testObject, MethodInfo method, string[] exampleHeaders, object[][] examples, int exampleRowIndex);
    }
}