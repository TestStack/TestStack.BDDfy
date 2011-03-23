using Bddify.Core;
using Bddify.Processors;
using Bddify.Reporters;
using Bddify.Scanners;

namespace $rootnamespace$
{
    public static class BddifyExtension
    {
        public static Bddifier Bddify(this object testObject)
        {
            var bddifier = new Bddifier(
                testObject,
                new GwtScanner(),
                new IProcessor[]
                { 
                    new TestRunner<InconclusiveException>(), //specify inconclusive exception of your testing framework of choice; e.g. InconclusiveException for nUnit and AssertInconclusiveException for MsTest
                    new ConsoleReporter(),
                    new HtmlReporter(),
                    new ExceptionHandler(Assert.Inconclusive) // provide an action that throws inconclusive exception; e.g. Assert.Inconclusive for nUnit and MsTest
                });

            bddifier.Run();
            return bddifier;
        }
    }
}