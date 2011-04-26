using System;
using Bddify.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace $rootnamespace$.Bddify.Samples.MsTest
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