using Bddify.Core;
using Bddify.Processors;
using Bddify.Reporters;
using Bddify.Scanners;
using NUnit.Framework;

namespace Bddify.Tests
{
    public static class BddifyNunit
    {
        public static Bddifier Bddify(this object testObject)
        {
            var bddifier = LazyBddify(testObject);
            bddifier.Run();
            return bddifier;
        }

        public static Bddifier LazyBddify(this object testObject)
        {
            var bddifier = new Bddifier(
                testObject,
                new GwtScanner(),
                new IProcessor[]
                { 
                    new TestRunner<InconclusiveException>(), 
                    new ConsoleReporter(),
                    new ExceptionHandler(Assert.Inconclusive)
                });

            return bddifier;
        }
    }
}