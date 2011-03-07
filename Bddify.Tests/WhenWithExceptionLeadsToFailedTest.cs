using System;
using NUnit.Framework;

namespace Bddify.Tests
{
    public class WhenWithExceptionLeadsToFailedTest
    {
        [Given]
        void TheWhenPartIsBroken()
        {
        }

        [When]
        void EverythingElseIsGood()
        {
            throw new Exception();
        }

        [Then]
        void TheTestFails()
        {
        }

        [Test]
        public void Execute()
        {
            Assert.Throws<AssertionException>(this.Bddify);
        }

    }
}