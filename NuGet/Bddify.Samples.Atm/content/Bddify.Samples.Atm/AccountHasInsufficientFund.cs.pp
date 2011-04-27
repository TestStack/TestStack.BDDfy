using NUnit.Framework;

namespace $rootnamespace$.Bddify.Samples.Atm
{
    public class AccountHasInsufficientFund
    {
        private Card _card;
        private Atm _atm;

        void GivenTheAccountBalanceIs10()
        {
            _card = new Card(true, 10);
        }

        void AndGivenTheCardIsValid()
        {

        }

        void AndGivenTheMachineContainsEnoughMoney()
        {
            _atm = new Atm(100);
        }

        void WhenTheAccountHolderRequests20()
        {
            _atm.RequestMoney(_card, 20);
        }

        void ThenTheAtmShouldNotDispenseAnyMoney()
        {
            Assert.That(_atm.DispenseValue, Is.EqualTo(0));
        }

        void AndTheAtmShouldSayThereAreInsufficientFunds()
        {
            Assert.That(_atm.Message, Is.EqualTo(DisplayMessage.InsufficientFunds));
        }

        void AndTheAccountBalanceShouldBe20()
        {
            Assert.That(_card.AccountBalance, Is.EqualTo(10));
        }

        void AndTheCardShouldBeReturned()
        {
            Assert.That(_atm.CardIsRetained, Is.False);
        }
    }
}