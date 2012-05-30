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
using System.Reflection;
using BDDfy.Core;

namespace BDDfy.Processors
{
    public class ExceptionProcessor : IProcessor
    {
        private readonly Action _assertInconclusive;
        private static readonly Action BestGuessInconclusiveAssertion;

        static readonly List<string> ExcludedAssemblies =
            new List<string>(new[] { "System", "mscorlib", "BDDfy", "TestDriven", "JetBrains.ReSharper" });
    
        static ExceptionProcessor()
        {
            var exceptionType = typeof(Exception);
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if(ExcludedAssemblies.Any(ex => assembly.GetName().FullName.StartsWith(ex)))
                    continue;

                foreach (var inconclusiveExceptionType in GetTypesSafely(assembly))
                {
                    if (inconclusiveExceptionType.Name.Contains("Inconclusive") &&
                        inconclusiveExceptionType.Name.Contains("Exception") &&
                        exceptionType.IsAssignableFrom(inconclusiveExceptionType))
                    {
                        var constructors = inconclusiveExceptionType.GetConstructors();
                        var shortestCtor = constructors.Min(c => c.GetParameters().Length);
                        var ctor = constructors.First(c => c.GetParameters().Length == shortestCtor);
                        var argList = new List<object>();
                        argList.AddRange(ctor.GetParameters().Select(p => DefaultValue(p.ParameterType)));
                        BestGuessInconclusiveAssertion = () => { throw (Exception)ctor.Invoke(argList.ToArray()); };
                        return;
                    }
                }
            }

            BestGuessInconclusiveAssertion = () => { throw new InconclusiveException(); };
        }

        private static IEnumerable<Type> GetTypesSafely(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types.Where(x => x != null);
            }
        }

        //http://stackoverflow.com/questions/520290/how-can-i-get-the-default-value-of-a-type-in-a-non-generic-way
        static object DefaultValue(Type myType)
        {
            return !myType.IsValueType ? null : Activator.CreateInstance(myType);
        }

        public ExceptionProcessor() : this(BestGuessInconclusiveAssertion)
        {
        }

        public ExceptionProcessor(Action assertInconclusive)
        {
            _assertInconclusive = assertInconclusive;
        }

        public ProcessType ProcessType
        {
            get { return ProcessType.ProcessExceptions; }
        }

        // http://weblogs.asp.net/fmarguerie/archive/2008/01/02/rethrowing-exceptions-and-preserving-the-full-call-stack-trace.aspx
        private static void PreserveStackTrace(Exception exception)
        {
            MethodInfo preserveStackTrace = typeof(Exception).GetMethod("InternalPreserveStackTrace",
              BindingFlags.Instance | BindingFlags.NonPublic);
            preserveStackTrace.Invoke(exception, null);
        }

        public void Process(Story story)
        {
            var allSteps = story.Scenarios.SelectMany(s => s.Steps);
            if (!allSteps.Any())
                return;

            var worseResult = story.Result;

            var stepWithWorseResult = allSteps.First(s => s.Result == worseResult);

            if (worseResult == StepExecutionResult.Failed || worseResult == StepExecutionResult.Inconclusive)
            {
                PreserveStackTrace(stepWithWorseResult.Exception);
                throw stepWithWorseResult.Exception;
            }

            if (worseResult == StepExecutionResult.NotImplemented)
                _assertInconclusive();
        }
    }
}