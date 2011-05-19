// This sample is to demonstrate how bddify deals with NotImplementedException.
// Bddify does not consider this exception as an error; instead it deals with it as an Inconclusive Exception.
// This allows you to do test first BDD; in other words, you can write your tests and throw NotImplementedException
// from the steps and/or classes under the test that have not been implemented yet and bddify shows the failing steps as 'Not Implemented'

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

		// This step will be indicated as 'Not Implemented'
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