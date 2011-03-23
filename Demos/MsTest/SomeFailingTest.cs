using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demos.MsTest
{
    [TestClass]
    public class SomeFailingTest
    {
        void GivenThisTestOrOneOfTheClassesItCallsToIsIncomplete()
        {
        }

        void WhenTheTestIsRun()
        {
            throw new ApplicationException("There was an error here");
        }

        void ThenItIsFlaggedAsIncomplete()
        {
            
        }

        [TestMethod]
        public void Execute()
        {
            this.Bddify();
        }
    }
}