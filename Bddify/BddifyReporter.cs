using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Bddify
{
    public class BddifyReporter : IBddifyReporter
    {
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

        public ExecutionResult ReportException(MethodInfo method, Exception exception)
        {
            PrintOutput(ReportByMethodInfo(method) + "  [Failed] ");
            PrintOutput("====================================");
            PrintOutput(exception.Message);
            PrintOutput(exception.StackTrace);
            PrintOutput("======== End of stack trace ========");
            return ExecutionResult.Failed;
        }

        public ExecutionResult ReportSuccess(MethodInfo method)
        {
            PrintOutput(ReportByMethodInfo(method));
            return ExecutionResult.Succeeded;
        }

        public ExecutionResult ReportNotImplemented(MethodInfo method)
        {
            PrintOutput(ReportByMethodInfo(method) + "  [Not Implemented] ");
            return ExecutionResult.NotImplemented;
        }

        public void ReportOnObjectUnderTest(object objectUnderTest)
        {
            PrintOutput("Scenario: " + CreateSentenceFromName(objectUnderTest.GetType().Name));
        }
    }
}