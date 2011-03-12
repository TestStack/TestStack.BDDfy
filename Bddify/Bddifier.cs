using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Bddify
{
    public class Bddifier
    {
        public static readonly Action<string> DefaultPrintOutput = Console.WriteLine;
        public static Action<string> PrintOutput = DefaultPrintOutput;

        public static Func<string, string> CreateSentenceFromUnderscoreSeparatedWords = methodName => string.Join(" ", methodName.Split(new[] { '_' }));
        public static Func<string, string> CreateSentenceFromCamelName = name => Regex.Replace(name, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
        public static Func<string, string> CreateSentenceFromName = name => CreateSentenceFromCamelName(CreateSentenceFromUnderscoreSeparatedWords(name));
        public static Func<string, string> CreateSentenceFromTypeName = CreateSentenceFromName;
        public static Func<MethodInfo, string> ReportByMethodInfo =
            method =>
            {
                var header = (ExecutableAttribute)method.GetCustomAttributes(typeof(ExecutableAttribute), false).FirstOrDefault();
                if (header == null)
                    return CreateSentenceFromName(method.Name);

                return string.Format("{0} {1}", header.Text.PadRight(header.TextPad), CreateSentenceFromName(method.Name));
            };

        private readonly Action _assertInconclusive;
        private readonly IResultProcessor _resultProcessor;
        private readonly object _testObject;
        private readonly IScanner _scanner;
        private readonly ITestRunner _runner;

        public Bddifier(
            Action assertInconclusive,
            IScanner scanner,
            ITestRunner runner,
            IResultProcessor resultProcessor, 
            object testObject)
        {
            _assertInconclusive = assertInconclusive;
            _resultProcessor = resultProcessor;
            _testObject = testObject;
            _scanner = scanner;
            _runner = runner;
        }

        public void Run()
        {
            var steps = _scanner.Scan(_testObject.GetType());
            Bddee = new Bddee(_testObject, steps);
            _runner.Run(Bddee);
            _resultProcessor.Process(Bddee);
            RethrowExceptions(Bddee);
        }

        private void RethrowExceptions(Bddee bddee)
        {
            var worseResult = (StepExecutionResult)bddee.Steps.Max(s => (int)s.Result);
            var stepWithWorseResult = bddee.Steps.First(s => s.Result == worseResult);
            if (worseResult == StepExecutionResult.Failed)
                throw stepWithWorseResult.Exception;

            if (worseResult == StepExecutionResult.Inconclusive)
                throw stepWithWorseResult.Exception;

            if (worseResult == StepExecutionResult.NotImplemented)
                _assertInconclusive();
        }

        public Bddee Bddee { get; private set; }
    }
}