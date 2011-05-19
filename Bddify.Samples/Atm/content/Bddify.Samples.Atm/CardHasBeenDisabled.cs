using NUnit.Framework;

namespace Bddify.Samples.Atm
{
    public class CardHasBeenDisabled
    {
        private Card _card;
        Atm _subject;

        void GivenTheCardIsDisabled()
        {
            _card = new Card(false, 100);
            _subject = new Atm(100);
        }

        void WhenTheAccountHolderRequestsMoney()
        {
            _subject.RequestMoney(_card, 20);
        }

        void ThenTheAtmShouldRetainTheCard()
        {
            Assert.IsTrue(_subject.CardIsRetained);
        }

        void AndTheAtmShouldSayTheCardHasBeenRetained()
        {
            Assert.AreEqual(_subject.Message, DisplayMessage.CardIsRetained);
        }
    }
}