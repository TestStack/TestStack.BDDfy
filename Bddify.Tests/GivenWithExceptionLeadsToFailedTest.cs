using System;
using NUnit.Framework;

namespace Bddify.Tests
{
    public class GivenWithExceptionLeadsToFailedTest
    {
        [Given]
        void TheGivenPartIsBroken()
        {
            throw new Exception();
        }

        [When]
        void EverythingElseIsGood()
        {
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