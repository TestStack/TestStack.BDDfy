using Bddify.Core;
using Bddify.Processors;
using NUnit.Framework;

namespace Demos.NUnit
{
    public static class BddifyNunit
    {
        public static Bddifier Bddify(this object testObject)
        {
            var bddifer = testObject.LazyBddify(new ExceptionProcessor(Assert.Inconclusive));
            bddifer.Run();
            return bddifer;
        }
    }
}