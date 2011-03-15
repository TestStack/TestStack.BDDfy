using System;
using System.Linq;

namespace Bddify
{
    public class ExceptionHandler : IProcessor
    {
        private readonly Action _assertInconclusive;

        public ExceptionHandler(Action assertInconclusive)
        {
            _assertInconclusive = assertInconclusive;
        }

        public ProcessType ProcessType
        {
            get { return ProcessType.HandleExceptions; }
        }

        public void Process(Bddee bddee)
        {
            var worseResult = bddee.Result;
            var stepWithWorseResult = bddee.Steps.First(s => s.Result == worseResult);
            if (worseResult == StepExecutionResult.Failed)
                throw stepWithWorseResult.Exception;

            if (worseResult == StepExecutionResult.Inconclusive)
                throw stepWithWorseResult.Exception;

            if (worseResult == StepExecutionResult.NotImplemented)
                _assertInconclusive();
        }
    }
}