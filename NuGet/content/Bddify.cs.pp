using Bddify.Core;
using Bddify.Processors;
using NUnit.Framework;

namespace $rootnamespace$
{
    public static class BddifyExtension
    {
        public static void Bddify(this object testObject)
        {
			var exceptionHandler = new ExceptionHandler(Assert.Inconclusive); // provide an action that throws inconclusive exception; e.g. Assert.Inconclusive for nUnit and MsTest
            testObject.Bddify(exceptionHandler); 
        }
    }
}