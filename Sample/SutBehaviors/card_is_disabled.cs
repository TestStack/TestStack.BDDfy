using SystemUnderTest;
using Bddify;
using NUnit.Framework;

namespace SutBehaviors
{
    public class card_is_disabled
    {
        private Card _card;
        Atm _subject;

        [Given]
        void the_card_is_disabled()
        {
            _card = new Card(false);
            _subject = new Atm();
        }

        [When]
        void the_account_holder_requests_money()
        {
            _subject.RequestMoney(_card);
        }

        [Then]
        void the_Atm_should_retain_the_card()
        {
            Assert.That(_subject.CardIsRetained, Is.True);
        }

        [AndThen]
        void the_Atm_should_say_the_card_has_been_retained()
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