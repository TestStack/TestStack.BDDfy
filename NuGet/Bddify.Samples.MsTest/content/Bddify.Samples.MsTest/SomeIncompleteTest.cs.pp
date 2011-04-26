using System;
using Bddify.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace $rootnamespace$.Bddify.Samples.MsTest
{
    [TestClass]
    public class SomeIncompleteTest
    {
        void GivenThisTestOrOneOfTheClassesItCallsToIsIncomplete()
        {
        }

        void WhenTheTestIsRun()
        {
            throw new NotImplementedException();
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