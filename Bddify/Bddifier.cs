using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bddify
{
    public class Bddifier
    {
        private readonly IBddifyReporter _reporter;
        private readonly IScanner _scanner;
        private readonly Exception _inconclusiveException;
        readonly object _instanceUnderTest;

        public Bddifier(
            IBddifyReporter reporter, 
            IScanner scanner, 
            Exception inconclusiveException, 
            object bddee)
        {
            _instanceUnderTest = bddee;
            _reporter = reporter;
            _scanner = scanner;
            _inconclusiveException = inconclusiveException;
        }

        public void Run()
        {
            _reporter.ReportOnObjectUnderTest(_instanceUnderTest);
            RunSteps(_scanner.Scan(_instanceUnderTest.GetType()));
        }

        private void RunSteps(IEnumerable<MethodInfo> methods)
        {
            var result = (ExecutionResult)methods.Max(m => (int)Execute(m));

            if (result == ExecutionResult.NotImplemented || result == ExecutionResult.Inconclusive)
                throw _inconclusiveException;
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

                if (ex.InnerException is NotImplementedException ||
                    ex.InnerException.GetType() == _inconclusiveException.GetType())
                {
                    _reporter.ReportNotImplemented(method, ex.InnerException);
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