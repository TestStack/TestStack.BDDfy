using System;
using System.Reflection;
using Bddify.Core;
using Bddify.Processors;
using Bddify.Reporters;
using Bddify.Scanners;
using System.Collections.Generic;
using System.Linq;

namespace Bddify
{
    public static class BddifyExtensions
    {
        public static void Bddify(this Assembly assmebly, Predicate<Type> shouldBddify, bool htmlReport = true, bool consoleReport = true)
        {
            foreach (var testType in assmebly.GetTypes().Where(t => shouldBddify(t)))
            {
                // should not handle exceptions in this case
                testType.Bddify(handleExceptions: false, htmlReport: htmlReport, consoleReport: consoleReport);
            }

            HtmlReporter.GenerateHtmlReport();
        }

        static IScanner GetDefaultScanner(string scenarioTextTemplate = null)
        {
            return new DefaultScanner(
                new ScanForScenarios(
                    new IScanForSteps[]
                    {
                        new DefaultScanForStepsByMethodName(),
                        new ExecutableAttributeScanner()
                    },
                    scenarioTextTemplate));
        }

        public static Story Bddify(this Type testType, IExceptionProcessor exceptionProcessor = null, bool handleExceptions = true, bool htmlReport = true, bool consoleReport = true, string scenarioTextTemplate = null, params IScanForSteps[] stepScanners)
        {
            IScanner scanner = null;

            if (stepScanners != null && stepScanners.Length > 0)
                scanner = new DefaultScanner(new ScanForScenarios(stepScanners, scenarioTextTemplate));

            return testType.LazyBddify(exceptionProcessor, handleExceptions, htmlReport, consoleReport, scanner).Run();
        }

        public static Bddifier LazyBddify(this Type testType, IExceptionProcessor exceptionProcessor = null, bool handleExceptions = true, bool htmlReport = true, bool consoleReport = true, IScanner scanner = null)
        {
            if(typeof(IScanForSteps).IsAssignableFrom(testType))
                throw new InvalidOperationException("You are calling a wrong overload of bddify. The method you are calling should be called on the test object; not on a scanner.");

            var processors = new List<IProcessor> {new TestRunner()};

            if(consoleReport)
                processors.Add(new ConsoleReporter());

            if(htmlReport)
                processors.Add(new HtmlReporter());

            if (handleExceptions)
            {
                if (exceptionProcessor == null)
                    exceptionProcessor = new ExceptionProcessor();

                processors.Add(exceptionProcessor);
            }

            var storyScanner = scanner ?? GetDefaultScanner();

            return new Bddifier(testType, storyScanner, processors);
        }
    }
}