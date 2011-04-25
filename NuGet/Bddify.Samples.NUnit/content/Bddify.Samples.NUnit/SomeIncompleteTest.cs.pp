using System;
using Bddify.Core;
using NUnit.Framework;

namespace $rootnamespace$
{
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

        [Test]
        public void Execute()
        {
            this.Bddify();
        }
    }
}