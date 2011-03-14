using Bddify;
using NUnit.Framework;

namespace SutBehaviors.AtmAndCardSamples
{
    public class CardIsDisabled
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
            Assert.That(_subject.CardIsRetained, Is.True);
        }

        void AndTheAtmShouldSayTheCardHasBeenRetained()
        {
            Assert.That(_subject.Message, Is.EqualTo(DisplayMessage.CardIsRetained));
            Assert.Inconclusive();
        }

        [Test]
        public void Execute()
        {
            this.Bddify<GwtScanner>();
        }
    }
}