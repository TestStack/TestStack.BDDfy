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
        private readonly IScanner _scanner;
        readonly object _instanceUnderTest;

        public Bddifier(IBddifyReporter reporter, IScanner scanner, object bddee)
        {
            _instanceUnderTest = bddee;
            _reporter = reporter;
            _scanner = scanner;
        }

        public void Run()
        {
            _reporter.ReportOnObjectUnderTest(_instanceUnderTest);
            RunSteps(_scanner.Scan(_instanceUnderTest.GetType()));
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
                {
                    _reporter.ReportNotImplemented(method);
                    return ExecutionResult.NotImplemented;
                }

                _reporter.ReportException(method, ex.InnerException);
                throw ex.InnerException;
            }

            _reporter.ReportSuccess(method);
            return ExecutionResult.Succeeded;
        }
    }
}