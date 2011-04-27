using Bddify.Scanners.GwtAttributes;
using NUnit.Framework;

namespace $rootnamespace$.Bddify.Samples.Atm
{
    public class AccountHasSufficientFunds
    {
        private Card _card;
        private Atm _atm;

        void GivenTheAccountBalanceIs100()
        {
            _card = new Card(true, 100);
        }

        void AndGivenTheMachineContainsEnoughMoney()
        {
            _atm = new Atm(200);
        }

        void WhenTheAccountHolderRequests20()
        {
            _atm.RequestMoney(_card, 20);
        }

        [Then(StepText = "Then the ATM should dispense $20")]
        void ThenTheAtmShouldDispense20()
        {
            Assert.That(_atm.DispenseValue, Is.EqualTo(20));
        }

        void AndTheAccountBalanceShouldBe80()
        {
            Assert.That(_card.AccountBalance, Is.EqualTo(80));
        }

        void AndTheCardShouldBeReturned()
        {
            Assert.That(_atm.CardIsRetained, Is.False);
        }
    }
}