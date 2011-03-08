using System;
using System.Reflection;

namespace Bddify
{
    public interface IBddifyReporter
    {
        ExecutionResult ReportException(MethodInfo method, Exception exception);
        ExecutionResult ReportSuccess(MethodInfo method);
        ExecutionResult ReportNotImplemented(MethodInfo method);
        void ReportOnObjectUnderTest(object objectUnderTest);
    }
}