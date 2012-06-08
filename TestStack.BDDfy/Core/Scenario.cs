// Copyright (C) 2011, Mehdi Khalili
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of the <organization> nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Linq;

namespace TestStack.BDDfy.Core
{
    public class Scenario
    {
        public Scenario(object testObject, IEnumerable<ExecutionStep> steps, string scenarioText)
        {
            TestObject = testObject;
            _steps = steps.OrderBy(o => o.ExecutionOrder).ToList();
            Id = Guid.NewGuid();

            Title = scenarioText;
        }

        public string Title { get; private set; }
        public TimeSpan Duration { get; set; }
        public object TestObject { get; private set; }
        public Guid Id { get; private set; }

        private readonly List<ExecutionStep> _steps;
        public IEnumerable<ExecutionStep> Steps
        {
            get { return _steps; }
        }

        public StepExecutionResult Result
        {
            get
            {
                if (!Steps.Any())
                    return StepExecutionResult.NotExecuted;

                return (StepExecutionResult)Steps.Max(s => (int)s.Result);
            }
        }

        // ToDo: this method does not really belong to this class
        public StepExecutionResult ExecuteStep(ExecutionStep executionStep)
        {
            try
            {
                executionStep.Execute(TestObject);
                executionStep.Result = StepExecutionResult.Passed;
            }
            catch (Exception ex)
            {
                // ToDo: more thought should be put into this. Is it safe to get the exception?
                var exception = ex.InnerException ?? ex;

                if (exception is NotImplementedException)
                {
                    executionStep.Result = StepExecutionResult.NotImplemented;
                    executionStep.Exception = exception;
                }
                else if (IsInconclusive(exception))
                {
                    executionStep.Result = StepExecutionResult.Inconclusive;
                    executionStep.Exception = exception;
                }
                else
                {
                    executionStep.Exception = exception;
                    executionStep.Result = StepExecutionResult.Failed;
                }
            }

            return executionStep.Result;
        }

        private static bool IsInconclusive(Exception exception)
        {
            return exception.GetType().Name.Contains("InconclusiveException");
        }
    }
}