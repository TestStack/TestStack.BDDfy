// This is just to show you how bddify deals with failing steps.
// Please note that because 'When' step is failing bddify will not run the 'Then' part
// That is because your assertions would not verify a valid state if the 'when' step has failed.
// Please note that in both console and html report 'Then' step is indicated as 'Not Executed'

using System;
using Bddify.Core;
using NUnit.Framework;

namespace $rootnamespace$.Bddify.Samples.NUnit
{
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

        [Test]
        public void Execute()
        {
            this.Bddify();
        }
    }
}