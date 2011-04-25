using System;
using Bddify.Core;
using NUnit.Framework;

namespace $rootnamespace$
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