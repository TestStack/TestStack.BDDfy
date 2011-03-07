using System;
using NUnit.Framework;

namespace Bddify.Tests
{
    public class NotImplementedThenIsInconclusive
    {
        [Given]
        void TheTestMethodIsNotFullyImplemented()
        {      
        }

        [When]
        void ThenPartThrowsNotImplementedException()
        {
        }
        
        [Then]
        void TheTestIsFlaggedAsInconclusive()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void Execute()
        {
            Assert.Throws<InconclusiveException>(() => this.Bddify());
        }
    }
}