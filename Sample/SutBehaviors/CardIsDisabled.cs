using SystemUnderTest;
using Bddify;
using NUnit.Framework;

namespace SutBehaviors
{
    public class CardIsDisabled
    {
        private Card _card;
        Atm _subject;

        [Given]
        void TheCardIsDisabled()
        {
            _card = new Card(false);
            _subject = new Atm();
        }

        [When]
        void TheAccountHolderRequestsMoney()
        {
            _subject.RequestMoney(_card);
        }

        [Then]
        void TheAtmShouldRetainTheCard()
        {
            Assert.That(_subject.CardIsRetained, Is.True);
        }

        [AndThen]
        void TheAtmShouldSayTheCardHasBeenRetained()
        {
            Assert.That(_subject.Message, Is.EqualTo(DisplayMessage.CardIsRetained));
        }

        [Test]
        public void Execute()
        {
            this.Bddify();
        }
    }
}