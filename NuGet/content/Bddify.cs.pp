using Bddify.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace $rootnamespace$
{
    public static class BddifyExtension
    {
        public static void Bddify(this object testObject)
        {
			// provide an action that throws inconclusive exception; e.g. Assert.Inconclusive for nUnit and MsTest
			var exceptionHandler = new ExceptionProcessor(Assert.Inconclusive); 

			// htmlReport and consoleReport are optional parameters that are set to true by default
			// you may set them to false if you want to deactivate them
            testObject.Bddify(exceptionHandler, htmlReport:true, consoleReport:true); 
        }
    }
}