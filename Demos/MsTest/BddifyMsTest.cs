using Bddify.Core;
using Bddify.Processors;
using Bddify.Reporters;
using Bddify.Scanners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demos.MsTest
{
    public static class BddifyMsTest
    {
        public static Bddifier Bddify<T>(this object testObject)
            where T : IScanner, new()
        {
            var bddifier = new Bddifier(
                testObject,
                new T(),
                new IProcessor[]
                { 
                    new TestRunner<AssertInconclusiveException>(), 
                    new ConsoleReporter(),
                    new HtmlReporter(),
                    new ExceptionHandler(Assert.Inconclusive)
                });

            bddifier.Run();
            return bddifier;
        }

        public static Bddifier Bddify(this object testObject)
        {
            return testObject.Bddify<GwtScanner>();
        }
    }
}