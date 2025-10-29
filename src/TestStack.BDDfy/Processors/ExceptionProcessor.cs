using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TestStack.BDDfy.Processors
{
    public class ExceptionProcessor(Action assertInconclusive): IProcessor
    {
        private readonly Action _assertInconclusive = assertInconclusive;
        private static readonly Action BestGuessInconclusiveAssertion;

        static readonly List<string> ExcludedAssemblies =
            new(new[] { "System", "mscorlib", "TestStack.BDDfy", "TestDriven", "JetBrains.ReSharper" });
    
        static ExceptionProcessor()
        {
            var exceptionType = typeof(Exception);

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
            return !myType.IsValueType() ? null : Activator.CreateInstance(myType);
        }

        public ExceptionProcessor() : this(BestGuessInconclusiveAssertion)
        {
        }

        public ProcessType ProcessType
        {
            get { return ProcessType.ProcessExceptions; }
        }

        // http://weblogs.asp.net/fmarguerie/archive/2008/01/02/rethrowing-exceptions-and-preserving-the-full-call-stack-trace.aspx
        internal static void PreserveStackTrace(Exception exception)
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

            if (worseResult == Result.Failed || worseResult == Result.Inconclusive)
            {
                PreserveStackTrace(stepWithWorseResult.Exception);
                throw stepWithWorseResult.Exception;
            }

            if (worseResult == Result.NotImplemented)
                _assertInconclusive();
        }
    }
}