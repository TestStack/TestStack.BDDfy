using System;
using System.Reflection;
using Bddify.Processors;
using Bddify.Reporters;
using Bddify.Scanners;
using System.Collections.Generic;

namespace Bddify.Core
{
    public static class Extensions
    {
        public static void Bddify(this Assembly assmebly, Predicate<Type> shouldBddify)
        {

        }

        public static void Bddify(this object testObject, IExceptionHandler exceptionHandler)
        {
            var bddifier = LazyBddify(testObject, exceptionHandler);
            bddifier.Run();
        }

        public static Bddifier LazyBddify(this object testObject, IExceptionHandler exceptionHandler, bool htmlReport = true, bool consoleReport = true)
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