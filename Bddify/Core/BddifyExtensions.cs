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
        public static void Bddify(this Assembly assmebly, Predicate<Type> shouldBddify)
        {
            foreach (var testObjectType in assmebly.GetTypes().Where(t => shouldBddify(t)))
            {
                var testObject = Activator.CreateInstance(testObjectType);
                testObject.Bddify();
            }
        }

        public static void Bddify(this object testObject, IExceptionHandler exceptionHandler = null)
        {
            var bddifier = LazyBddify(testObject, exceptionHandler);
            bddifier.Run();
        }

        public static Bddifier LazyBddify(this object testObject, IExceptionHandler exceptionHandler = null, bool htmlReport = true, bool consoleReport = true)
        {
            var processors = new List<IProcessor> {new TestRunner()};

            if(consoleReport)
                processors.Add(new ConsoleReporter());

            if(htmlReport)
                processors.Add(new HtmlReporter());

            if (exceptionHandler != null)
                processors.Add(exceptionHandler);

            return new Bddifier(testObject, new DefaultMethodNameScanner(), processors);
        }
    }
}