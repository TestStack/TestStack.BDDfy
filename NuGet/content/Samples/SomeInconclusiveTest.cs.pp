using System;
using Bddify.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace $rootnamespace$
{
    [TestClass]
    public class SomeInconclusiveTest
    {
        void GivenThisTestThrowsInconclusiveException()
        {
        }

        void WhenTheTestIsRun()
        {
        }

        void ThenItIsFlaggedAsInconclusive()
        {
			Assert.Inconclusive();            
        }

        [TestMethod]
        public void Execute()
        {
            this.Bddify();
        }
    }
}