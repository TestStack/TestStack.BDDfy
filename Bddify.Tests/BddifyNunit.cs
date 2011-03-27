using Bddify.Core;
using Bddify.Processors;
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
            return testObject.LazyBddify(new ExceptionHandler(Assert.Inconclusive));
        }
    }
}