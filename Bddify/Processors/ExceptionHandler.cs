using System;
using System.Linq;
using Bddify.Core;

namespace Bddify.Processors
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

        public void Process(Scenario scenario)
        {
            var worseResult = scenario.Result;
            
            if(!scenario.Steps.Any())
                return;

            var stepWithWorseResult = scenario.Steps.First(s => s.Result == worseResult);
            if (worseResult == StepExecutionResult.Failed)
                throw stepWithWorseResult.Exception;

            if (worseResult == StepExecutionResult.Inconclusive)
                throw stepWithWorseResult.Exception;

            if (worseResult == StepExecutionResult.NotImplemented)
                _assertInconclusive();
        }
    }
}