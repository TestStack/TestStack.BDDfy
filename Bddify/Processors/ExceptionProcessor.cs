using System;
using System.Linq;
using Bddify.Core;

namespace Bddify.Processors
{
    public class ExceptionProcessor : IExceptionProcessor
    {
        private readonly Action _assertInconclusive;

        public ExceptionProcessor(Action assertInconclusive)
        {
            _assertInconclusive = assertInconclusive;
        }

        public ProcessType ProcessType
        {
            get { return ProcessType.ProcessExceptions; }
        }

        public void Process(Story story)
        {
            var worseResult = story.Result;
            
            var stepWithWorseResult = story.Scenarios.SelectMany(s => s.Steps).First(s => s.Result == worseResult);
            if (worseResult == StepExecutionResult.Failed)
                throw stepWithWorseResult.Exception;

            if (worseResult == StepExecutionResult.Inconclusive)
                throw stepWithWorseResult.Exception;

            if (worseResult == StepExecutionResult.NotImplemented)
                _assertInconclusive();
        }
    }
}