using Bddify.Core;
using Bddify.Processors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demos.MsTest
{
    public static class BddifyMsTest
    {
        public static Bddifier Bddify(this object testObject)
        {
            var bddifier = testObject.LazyBddify(new ExceptionProcessor(Assert.Inconclusive));
            bddifier.Run();
            return bddifier;
        }
    }
}