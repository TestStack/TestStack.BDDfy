using System;
using NUnit.Framework;

namespace Bddify.Tests
{
    public class NotImplementedGivenIsInconclusive
    {
        [Given]
        void TheTestMethodIsNotFullyImplemented()
        {      
            throw new NotImplementedException();
        }

        [When]
        void GivenPartThrowsNotImplementedException()
        {
            
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