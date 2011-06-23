using System;
using Bddify.Core;
using Bddify.Processors;
using Bddify.Reporters;
using Bddify.Scanners;
using System.Collections.Generic;

namespace Bddify
{
    public static class BddifyExtensions
    {
#if NET35
        public static Story Bddify(this Type testType)
        {
            return Bddify(testType, null, true, false, true, null);
        }
        
        public static Bddifier LazyBddify(this Type testType)
        {
            return LazyBddify(testType, false, null, true, true, null);
        }
#endif

#if !NET35
        static IScanner GetDefaultScanner(string scenarioTextTemplate = null)
#else
        static IScanner GetDefaultScanner()
#endif
    {
            return new DefaultScanner(
                new ScanForScenarios(
                    new IScanForSteps[]
                    {
                        new DefaultMethodNameStepScanner(),
                        new ExecutableAttributeStepScanner()
                    }));
        }

        public static Story Bddify(this Type testType, IExceptionProcessor exceptionProcessor = null, bool handleExceptions = true, bool htmlReport = true, bool consoleReport = true, string scenarioTextTemplate = null, params IScanForSteps[] stepScanners)
        {
            IScanner scanner = null;

            if (stepScanners != null && stepScanners.Length > 0)
                scanner = new DefaultScanner(new ScanForScenarios(stepScanners, scenarioTextTemplate));

            return testType.LazyBddify(true, exceptionProcessor, handleExceptions, consoleReport, scanner).Run();
        }

        public static Bddifier LazyBddify(this Type testType, string scenarioTextTemplate = null, params IScanForSteps[] stepScanners)
        {
            IScanner scanner = null;

            if (stepScanners != null && stepScanners.Length > 0)
                scanner = new DefaultScanner(new ScanForScenarios(stepScanners, scenarioTextTemplate));

            return testType.LazyBddify(true, scanner:scanner);
        }

        public static Bddifier LazyBddify(this Type testType, bool htmlReport, IExceptionProcessor exceptionProcessor = null, bool handleExceptions = true, bool consoleReport = true, IScanner scanner = null)
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