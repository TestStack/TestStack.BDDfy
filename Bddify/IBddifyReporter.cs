using System;
using System.Reflection;

namespace Bddify
{
    public interface IBddifyReporter
    {
        void ReportException(MethodInfo method, Exception exception);
        void ReportSuccess(MethodInfo method);
        void ReportNotImplemented(MethodInfo method, Exception notImplementedException);
        void ReportOnObjectUnderTest(object objectUnderTest);
    }
}