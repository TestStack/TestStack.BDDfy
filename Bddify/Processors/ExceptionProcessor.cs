using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Bddify.Core;

namespace Bddify.Processors
{
    public class ExceptionProcessor : IExceptionProcessor
    {
        private readonly Action _assertInconclusive;
        private static readonly Action BestGuessInconclusiveAssertion;

        static readonly List<string> ExcludedAssemblies =
            new List<string>(new[] { "System", "mscorlib", "Bddify", "TestDriven", "JetBrains.ReSharper" });
    
        static ExceptionProcessor()
        {
#if !SILVERLIGHT
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
#endif

            BestGuessInconclusiveAssertion = () => { throw new InconclusiveException(); };
        }

        //ToDo: this is rather hacky and has to be fixed
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
#if !SILVERLIGHT
            MethodInfo preserveStackTrace = typeof(Exception).GetMethod("InternalPreserveStackTrace",
              BindingFlags.Instance | BindingFlags.NonPublic);
            preserveStackTrace.Invoke(exception, null);
#endif
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