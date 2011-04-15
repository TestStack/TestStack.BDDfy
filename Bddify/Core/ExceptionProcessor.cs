using System;
using System.Collections.Generic;
using System.Linq;

namespace Bddify.Core
{
    public class ExceptionProcessor : IExceptionProcessor
    {
        private readonly Action _assertInconclusive;

        public ExceptionProcessor(Action assertInconclusive)
        {
            _assertInconclusive = assertInconclusive;
        }

        public void ProcessExceptions(IEnumerable<Scenario> scenarios)
        {
            var worseResult = (StepExecutionResult)scenarios.Max(s => (int)s.Result);
            
            var stepWithWorseResult = scenarios.SelectMany(s => s.Steps).First(s => s.Result == worseResult);
            if (worseResult == StepExecutionResult.Failed)
                throw stepWithWorseResult.Exception;

            if (worseResult == StepExecutionResult.Inconclusive)
                throw stepWithWorseResult.Exception;

            if (worseResult == StepExecutionResult.NotImplemented)
                _assertInconclusive();
        }
    }
}