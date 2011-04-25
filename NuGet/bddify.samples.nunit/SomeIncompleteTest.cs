using System;
using Bddify.Core;
using NUnit.Framework;

namespace Demos.NUnit
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