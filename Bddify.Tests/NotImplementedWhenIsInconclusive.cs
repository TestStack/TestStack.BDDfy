using System;
using NUnit.Framework;

namespace Bddify.Tests
{
    public class NotImplementedWhenIsInconclusive
    {
        [Given]
        void TheTestMethodIsNotFullyImplemented()
        {      
        }

        [When]
        void WhenPartThrowsNotImplementedException()
        {
            throw new NotImplementedException();
        }
        
        [Then]
        void TheTestIsFlaggedAsInconclusive()
        {
        }

        [Test]
        public void Execute()
        {
            Assert.Throws<InconclusiveException>(() => this.Bddify());
        }
    }
}