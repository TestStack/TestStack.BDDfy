using System;
using Bddify.Core;
using Bddify.Processors;
using Bddify.Reporters;
using Bddify.Scanners;
using System.Collections.Generic;
using Bddify.Scanners.ScenarioScanners;
using Bddify.Scanners.StepScanners;
using Bddify.Scanners.StepScanners.ExecutableAttribute;
using Bddify.Scanners.StepScanners.MethodName;

namespace Bddify
{
    public static class BddifyExtensions
    {
        static IScanner GetDefaultScanner(string scenarioTitle = null)
        {
            return new DefaultScanner(
                new ScanForScenarios(
                    new IScanForSteps[]
                    {
                        new DefaultMethodNameStepScanner(),
                        new ExecutableAttributeStepScanner()
                    }, scenarioTitle));
        }

        public static Story Bddify(this object testObject, string scenarioTitle = null, params IScanForSteps[] stepScanners)
        {
            return testObject.LazyBddify(scenarioTitle, stepScanners).Run();
        }

        public static Story Bddify(this object testObject)
        {
            return Bddify(testObject, null);
        }

        public static Bddifier LazyBddify(this object testObject)
        {
            return LazyBddify(testObject, null);
        }

        public static Bddifier LazyBddify(this object testObject, string scenarioTitle, params IScanForSteps[] stepScanners)
        {
            IScanner scanner = null;

            if (stepScanners != null && stepScanners.Length > 0)
                scanner = new DefaultScanner(new ScanForScenarios(stepScanners, scenarioTitle));

            return testObject.LazyBddify(scenarioTitle: scenarioTitle, exceptionProcessor: null, handleExceptions: true, consoleReport: true, htmlReport: true, scanner: scanner);
        }

        public static Bddifier LazyBddify(this object testObject, string scenarioTitle = null, IExceptionProcessor exceptionProcessor = null, bool handleExceptions = true, bool consoleReport = true, bool htmlReport = true, IScanner scanner = null)
        {
            if (testObject is Type)
                throw new ArgumentException("testObject should be an instance of a test class not its type");

            if (typeof(IScanForSteps).IsAssignableFrom(testObject.GetType()))
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

            var storyScanner = scanner ?? GetDefaultScanner(scenarioTitle);

            return new Bddifier(testObject, storyScanner, processors);
        }
    }
}