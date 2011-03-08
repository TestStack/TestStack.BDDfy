using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace Bddify
{
    public class Bddifier
    {
        private readonly IBddifyReporter _reporter;
        readonly object _instanceUnderTest;

        public Bddifier(IBddifyReporter reporter, object bddee)
        {
            _instanceUnderTest = bddee;
            _reporter = reporter;
        }

        public void Run()
        {
            _reporter.ReportOnObjectUnderTest(_instanceUnderTest);
            RunSteps(ScanAssembly(_instanceUnderTest.GetType()));
        }

        protected virtual IEnumerable<MethodInfo> ScanAssembly(Type type)
        {
            return type
                .GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.GetCustomAttributes(typeof(ExecutableAttribute), false).Any())
                .OrderBy(m => ((ExecutableAttribute)m.GetCustomAttributes(typeof(ExecutableAttribute), false)[0]).Order);
        }

        private void RunSteps(IEnumerable<MethodInfo> methods)
        {
            var result = (ExecutionResult)methods.Max(m => (int)Execute(m));

            if (result == ExecutionResult.NotImplemented)
                Assert.Inconclusive();
            else if (result == ExecutionResult.Failed)
                Assert.Fail();
        }

        private ExecutionResult Execute(MethodInfo method)
        {
            try
            {
                method.Invoke(_instanceUnderTest, null);
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    throw;

                if (ex.InnerException is NotImplementedException)
                    return _reporter.ReportNotImplemented(method);

                return _reporter.ReportException(method, ex.InnerException);
            }

            return _reporter.ReportSuccess(method);
        }
    }
}