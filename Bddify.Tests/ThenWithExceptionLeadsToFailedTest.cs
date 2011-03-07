using System;
using NUnit.Framework;

namespace Bddify.Tests
{
    public class ThenWithExceptionLeadsToFailedTest
    {
        [Given]
        void TheThenPartIsBroken()
        {
        }

        [When]
        void EverythingElseIsGood()
        {
        }

        [Then]
        void TheTestFails()
        {
            throw new Exception();
        }

        [Test]
        public void Execute()
        {
            Assert.Throws<AssertionException>(this.Bddify);
        }

    }
}