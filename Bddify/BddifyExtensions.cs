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
            foreach (var testObjectType in assmebly.GetTypes().Where(t => shouldBddify(t)))
            {
                var testObject = Activator.CreateInstance(testObjectType);
                // should not handle exceptions in this case
                testObject.Bddify(handleExceptions: false, htmlReport: htmlReport, consoleReport: consoleReport);
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

        public static Story Bddify(this object testObject, IExceptionProcessor exceptionProcessor = null, bool handleExceptions = true, bool htmlReport = true, bool consoleReport = true, string scenarioTextTemplate = null, params IScanForSteps[] stepScanners)
        {
            IScanner scanner = null;

            if (stepScanners != null && stepScanners.Length > 0)
                scanner = new DefaultScanner(new ScanForScenarios(stepScanners, scenarioTextTemplate));

            return testObject.LazyBddify(exceptionProcessor, handleExceptions, htmlReport, consoleReport, scanner).Run();
        }

        public static Story Bddify(this object testObject, Action assertInconclusive, bool htmlReport = true, bool consoleReport = true, string scenarioTextTemplate = null)
        {
            return testObject.Bddify(new ExceptionProcessor(assertInconclusive), htmlReport, consoleReport, scenarioTextTemplate: scenarioTextTemplate);
        }

        public static Bddifier LazyBddify(this object testObject, IExceptionProcessor exceptionProcessor = null, bool handleExceptions = true, bool htmlReport = true, bool consoleReport = true, IScanner scanner = null)
        {
            if(testObject is IScanForSteps)
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

            return new Bddifier(testObject, storyScanner, processors);
        }
    }
}