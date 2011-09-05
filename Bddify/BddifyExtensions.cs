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
                testObject,
                new ReflectiveScenarioScanner(
                    new IStepScanner[]
                    {
                        new ExecutableAttributeStepScanner(),
                        new DefaultMethodNameStepScanner(testObject)
                    }, scenarioTitle));
        }

        public static Story Bddify(this object testObject)
        {
            return Bddify(testObject, null);
        }

        public static Story Bddify(this object testObject, string scenarioTitle = null)
        {
            return testObject.LazyBddify(scenarioTitle).Run();
        }

        public static Story Bddify(this object testObject, string scenarioTitle = null, string htmlReportName = null)
        {
            return testObject.LazyBddify(scenarioTitle, htmlReportName:htmlReportName).Run();
        }

        public static Bddifier LazyBddify(this object testObject, string scenarioTitle = null, bool consoleReport = true, bool htmlReport = true, string htmlReportName = null)
        {
            IScanner scanner = null;
            var hasScanner = testObject as IHasScanner;

            if (hasScanner != null)
            {
                scanner = hasScanner.GetScanner(scenarioTitle);
                testObject = hasScanner.TestObject;
            }

            var processors = new List<IProcessor> { new TestRunner() };

            if (consoleReport)
                processors.Add(new ConsoleReporter());

            if (htmlReport)
                processors.Add(new HtmlReporter(htmlReportName));

            processors.Add(new ExceptionProcessor());

            var storyScanner = scanner ?? GetDefaultScanner(testObject, scenarioTitle);

            return new Bddifier(testObject, storyScanner, processors);
        }
    }
}