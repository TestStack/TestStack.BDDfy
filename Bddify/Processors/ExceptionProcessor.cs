using System;
using System.Collections.Generic;
using System.Linq;
using Bddify.Core;

namespace Bddify.Processors
{
    public class ExceptionProcessor : IExceptionProcessor
    {
        private readonly Action _assertInconclusive;
        private static readonly Action BestGuessInconclusiveAssertion;

        static ExceptionProcessor()
        {
            var exceptionType = typeof(Exception);
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if(assembly.GetName().FullName.StartsWith("System") || // exclude system assemblies
                    assembly.Equals(typeof(ExceptionProcessor).Assembly) ||
                    assembly.GetName().FullName.StartsWith("mscorlib"))
                    continue;

                foreach (var inconclusiveExceptionType in assembly.GetTypes())
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