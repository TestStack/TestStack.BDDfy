using Bddify;
using NUnit.Framework;

namespace SutBehaviors
{
    public static class BddifyNunit
    {
        public static Bddifier Bddify(this object testObject)
        {
            var bddifier = new Bddifier(
                Assert.Inconclusive,
                new GwtScanner(), 
                new TestRunner<InconclusiveException>(), 
                new ConsoleReporter(), 
                testObject);

            bddifier.Run();
            return bddifier;
        }
    }
}