using Bddify.Scanners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demos.MsTest.AtmAndCardSamples
{
    [TestClass]
    public class CardIsDisabledInconclusiveTest
    {
        private Card _card;
        Atm _subject;

        void GivenTheCardIsDisabled()
        {
            _card = new Card(false);
            _subject = new Atm();
        }

        void WhenTheAccountHolderRequestsMoney()
        {
            _subject.RequestMoney(_card);
        }

        void ThenTheAtmShouldRetainTheCard()
        {
            Assert.IsTrue(_subject.CardIsRetained);
        }

        void AndTheAtmShouldSayTheCardHasBeenRetained()
        {
            Assert.AreEqual(_subject.Message, DisplayMessage.CardIsRetained);
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Execute()
        {
            this.Bddify<GwtScanner>();
        }
    }
}