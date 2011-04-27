using NUnit.Framework;

namespace $rootnamespace$.Bddify.Samples.Atm
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
            Assert.That(_subject.CardIsRetained, Is.True);
        }

        void AndTheAtmShouldSayTheCardHasBeenRetained()
        {
            Assert.That(_subject.Message, Is.EqualTo(DisplayMessage.CardIsRetained));
        }
    }
}