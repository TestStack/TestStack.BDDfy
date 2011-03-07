using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Bddify
{
    public class Bddifier
    {
        protected enum ExecutionResult
        {
            Succeeded = 0,
            Failed = 1,
            NotImplemented = 2
        }

        object _instanceUnderTest;

        public static readonly Action<string> DefaultPrintOutput = Console.WriteLine;
        public static Action<string> PrintOutput = DefaultPrintOutput;
        
        public static Func<string, string> CreateSentenceFromUnderscoreSeparatedWords = methodName => string.Join(" ", methodName.Split(new[] { '_' }));
        public static Func<string, string> CreateSentenceFromCamelName = name => Regex.Replace(name, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
        public static Func<string, string> CreateSentenceFromName = name => CreateSentenceFromCamelName(CreateSentenceFromUnderscoreSeparatedWords(name));

        public static Func<MethodInfo, string> ReportByMethodInfo =
            method =>
            {
                var header = (ExecutableAttribute)method.GetCustomAttributes(typeof(ExecutableAttribute), false).Single();
                return string.Format("{0} {1}", header.Text.PadRight(header.TextPad), CreateSentenceFromName(method.Name));
            };

        public void Run(object bddee)
        {
            _instanceUnderTest = bddee;
            ReportOnType();
            RunSteps(ScanAssembly(_instanceUnderTest.GetType()));
        }

        private void RunSteps(IEnumerable<MethodInfo> methods)
        {
            var result = (ExecutionResult)methods.Max(m => (int)Execute(m));

            if (result == ExecutionResult.NotImplemented)
                Assert.Inconclusive();
            else if (result == ExecutionResult.Failed)
                Assert.Fail();
        }

        protected virtual void ReportOnType()
        {
            PrintOutput("Scenario: " + CreateSentenceFromName(_instanceUnderTest.GetType().Name));
            PrintOutput(string.Empty); // print an empty line
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

        protected virtual IEnumerable<MethodInfo> ScanAssembly(Type type)
        {
            return type
                .GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.GetCustomAttributes(typeof(ExecutableAttribute), false).Any())
                .OrderBy(m => ((ExecutableAttribute)m.GetCustomAttributes(typeof(ExecutableAttribute), false)[0]).Order);
        }

        protected virtual void Report(MethodInfo method, ExecutionResult result, Exception exception)
        {
            var message = ReportByMethodInfo(method);

            if (result != ExecutionResult.Succeeded)
                message = message.PadRight(70) + " => " + result;

            PrintOutput(message);

            if (exception != null)
            {
                PrintOutput(exception.Message);
                PrintOutput(exception.StackTrace);
                PrintOutput("===== End of stack trace =====");
                PrintOutput(string.Empty); // print an empty line
            }
        }
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