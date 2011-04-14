using Bddify.Scanners;
using Bddify.Scanners.GwtAttributes;
using NUnit.Framework;

namespace Demos.NUnit.AtmAndCardSamples
{
    public class Card_is_disabled_passing_test
    {
        private Card _card;
        Atm _subject;

        [Given]
        void Given_the_card_is_disabled()
        {
            _card = new Card(false);
            _subject = new Atm();
        }

        [When]
        void When_the_account_holder_requests_money()
        {
            _subject.RequestMoney(_card);
        }

        [Then]
        void Then_the_Atm_should_retain_the_card()
        {
            Assert.That(_subject.CardIsRetained, Is.True);
        }

        [AndThen]
        void and_the_Atm_should_say_the_card_has_been_retained()
        {
            Assert.That(_subject.Message, Is.EqualTo(DisplayMessage.CardIsRetained));
        }

        [Test]
        public void Execute()
        {
            this.Bddify<ExecutableAttributeScanner>();
        }
    }
}