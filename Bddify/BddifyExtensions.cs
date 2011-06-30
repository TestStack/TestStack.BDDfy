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
#if NET35
        public static Story Bddify(this object testObject)
        {
            return Bddify(testObject, null, true, false, true, null);
        }
        
        public static Bddifier LazyBddify(this object testObject)
        {
            return LazyBddify(testObject, false, null, true, true, null);
        }
#endif

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

        public static Story Bddify(this object testObject, IExceptionProcessor exceptionProcessor = null, bool handleExceptions = true, bool htmlReport = true, bool consoleReport = true, string scenarioTitle = null, params IScanForSteps[] stepScanners)
        {
            if(testObject is Type)
                throw new ArgumentException("testObject should be an instance of a test class not its type");

            IScanner scanner = null;

            if (stepScanners != null && stepScanners.Length > 0)
                scanner = new DefaultScanner(new ScanForScenarios(stepScanners, scenarioTitle));

            return testObject.LazyBddify(true, exceptionProcessor, handleExceptions, consoleReport, scanner, scenarioTitle).Run();
        }

        public static Bddifier LazyBddify(this object testObject, string scenarioTitle = null, params IScanForSteps[] stepScanners)
        {
            IScanner scanner = null;

            if (stepScanners != null && stepScanners.Length > 0)
                scanner = new DefaultScanner(new ScanForScenarios(stepScanners, scenarioTitle));

            return testObject.LazyBddify(true, scanner:scanner, scenarioTitle:scenarioTitle);
        }

        public static Bddifier LazyBddify(this object testObject, bool htmlReport, IExceptionProcessor exceptionProcessor = null, bool handleExceptions = true, bool consoleReport = true, IScanner scanner = null, string scenarioTitle = null)
        {
            if(typeof(IScanForSteps).IsAssignableFrom(testObject.GetType()))
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