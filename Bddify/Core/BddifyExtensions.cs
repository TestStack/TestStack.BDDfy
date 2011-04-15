using System;
using System.Reflection;
using Bddify.Processors;
using Bddify.Reporters;
using Bddify.Scanners;
using System.Collections.Generic;
using System.Linq;

namespace Bddify.Core
{
    public static class BddifyExtensions
    {
        public static void Bddify(this Assembly assmebly, Predicate<Type> shouldBddify, bool htmlReport = true, bool consoleReport = true)
        {
            foreach (var testObjectType in assmebly.GetTypes().Where(t => shouldBddify(t)))
            {
                var testObject = Activator.CreateInstance(testObjectType);
                IExceptionProcessor processor = null; // I should not throw exceptions because it will stop the assembly runner
                testObject.Bddify(processor, htmlReport:htmlReport, consoleReport:consoleReport);
            }

            HtmlReporter.GenerateHtmlReport();
        }

        public static void Bddify(this object testObject, IExceptionProcessor exceptionProcessor, bool htmlReport = true, bool consoleReport = true)
        {
            testObject.LazyBddify(exceptionProcessor, htmlReport, consoleReport).Run();
        }

        public static Bddifier LazyBddify(this object testObject, IExceptionProcessor exceptionProcessor = null, bool htmlReport = true, bool consoleReport = true)
        {
            var processors = new List<IProcessor> {new TestRunner()};

            if(consoleReport)
                processors.Add(new ConsoleReporter());

            if(htmlReport)
                processors.Add(new HtmlReporter());

            return new Bddifier(testObject, new DefaultMethodNameScanner(), exceptionProcessor, processors);
        }
    }
}