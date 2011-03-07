using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace Bddify
{
    public class Bddifier
    {
        enum ExecutionResult
        {
            Succeeded = 0,
            Failed = 1,
            NotImplemented = 2
        }

        object _instanceUnderTest;

        public static readonly Action<string> DefaultPrintOutput = Console.WriteLine;
        public static Action<string> PrintOutput = DefaultPrintOutput;
        public static Func<string, string> CreateSentenceFromUnderscoreSeparatedWords = methodName => string.Join(" ", methodName.Split(new[] { '_' }));
        public static Func<string, string> CreateSentenceFromName = CreateSentenceFromUnderscoreSeparatedWords;

        public void Run(object bddee)
        {
            _instanceUnderTest = bddee;
            Execute();
        }

        private void Execute()
        {
            PrintOutput("Scenario: " + CreateSentenceFromName(_instanceUnderTest.GetType().Name) + Environment.NewLine);
            var methods = ScanAssembly(_instanceUnderTest.GetType());
            var result = methods.Aggregate(ExecutionResult.Succeeded, (current, method) => (ExecutionResult)Math.Max((int)current, (int)Execute(method)));
            
            if(result == ExecutionResult.NotImplemented)
                Assert.Inconclusive();
            else if(result == ExecutionResult.Failed)
                Assert.Fail();
        }

        private ExecutionResult Execute(MethodInfo method)
        {
            var result = ExecutionResult.Succeeded;
            Exception execException = null;

            try
            {
                method.Invoke(_instanceUnderTest, null);
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    throw;

                if (ex.InnerException is NotImplementedException)
                    result = ExecutionResult.NotImplemented;
                else
                {
                    execException = ex.InnerException;
                    result = ExecutionResult.Failed;
                }
            }

            Report(method, result, execException);
            return result;
        }

        private static IEnumerable<MethodInfo> ScanAssembly(Type type)
        {
            return type
                .GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.GetCustomAttributes(typeof(ExecutableAttribute), false).Any())
                .OrderBy(m => ((ExecutableAttribute)m.GetCustomAttributes(typeof(ExecutableAttribute), false)[0]).Order);
        }

        private static void Report(MethodInfo method, ExecutionResult result, Exception exception)
        {
            var message = ReportByMethodInfo(method);

            if (result != ExecutionResult.Succeeded)
                message = message.PadRight(70) + " => " + result;

            PrintOutput(message);

            if (exception != null)
            {
                if (!typeof(AssertionException).IsAssignableFrom(exception.GetType()))
                    PrintOutput("There was an exception:");

                PrintOutput(exception.Message);
                PrintOutput(exception.StackTrace);
                PrintOutput("===== End of stack trace =====" + Environment.NewLine);
            }
        }

        public static Func<MethodInfo, string> ReportByMethodInfo =
            method =>
            {
                var header = (ExecutableAttribute)method.GetCustomAttributes(typeof(ExecutableAttribute), false).Single();
                var methodText = string.Join(Environment.NewLine, header.Text);
                return string.Format("{0} {1}", methodText.PadRight(header.TextPad), CreateSentenceFromName(method.Name));
            };
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ExecutableAttribute : Attribute
    {
        public ExecutableAttribute(int order, string text)
        {
            Text = text;
            Order = order;
        }

        public int Order { get; private set; }
        public string Text { get; private set; }
        public int TextPad { get; set; }
    }
}