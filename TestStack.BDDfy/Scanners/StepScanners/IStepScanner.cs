using System.Collections.Generic;
using System.Reflection;

namespace TestStack.BDDfy
{
    public interface IStepScanner
    {
        IEnumerable<Step> Scan(ITestContext testContext, MethodInfo method);
        IEnumerable<Step> Scan(ITestContext testContext, MethodInfo method, Example example);
    }
}