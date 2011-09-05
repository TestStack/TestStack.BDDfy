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
        static IScanner GetDefaultScanner(object testObject, string scenarioTitle = null)
        {
            return new DefaultScanner(
                new ScanForScenario(
                    new IScanForSteps[]
                    {
                        new ExecutableAttributeStepScanner(),
                        new DefaultMethodNameStepScanner(testObject)
                    }, scenarioTitle));
        }

        public static Story Bddify(this object testObject, string scenarioTitle = null)
        {
            return testObject.LazyBddify(scenarioTitle).Run();
        }

        public static Story Bddify(this object testObject)
        {
            return Bddify(testObject, null);
        }

        static void VerifyTestObject(object testObject)
        {
            if (testObject is Type)
                throw new ArgumentException("testObject should be an instance of a test class not its type");

            if (typeof(IScanForSteps).IsAssignableFrom(testObject.GetType()))
                throw new InvalidOperationException("You are calling a wrong overload of bddify. The method you are calling should be called on the test object; not on a scanner.");
        }

        public static Bddifier LazyBddify(this object testObject, IScanner scanner, bool consoleReport = true, bool htmlReport = true)
        {
            return LazyBddify(testObject, null, scanner, consoleReport, htmlReport);
        }

        public static Bddifier LazyBddify(this object testObject, string scenarioTitle = null, bool consoleReport = true, bool htmlReport = true)
        {
            return LazyBddify(testObject, scenarioTitle, null, consoleReport, htmlReport);
        }

        static Bddifier LazyBddify(this object testObject, string scenarioTitle, IScanner scanner, bool consoleReport = true, bool htmlReport = true)
        {
            VerifyTestObject(testObject);

            var processors = new List<IProcessor> { new TestRunner() };

            if (consoleReport)
                processors.Add(new ConsoleReporter());

            if (htmlReport)
                processors.Add(new HtmlReporter());

            processors.Add(new ExceptionProcessor());

            var storyScanner = scanner ?? GetDefaultScanner(testObject, scenarioTitle);

            return new Bddifier(testObject, storyScanner, processors);
        }
    }
}