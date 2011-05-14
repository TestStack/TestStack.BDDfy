using Bddify.Scanners.GwtAttributes;
using NUnit.Framework;

namespace $rootnamespace$.Bddify.Samples.Atm
{
    public class AccountHasInsufficientFund
    {
        private Card _card;
        private Atm _atm;

		// You can override step text using executable attributes
		[Given(StepText = "Given the account balance is $10")]
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

		[When(StepText = "When the account holder requests $20")]
        void WhenTheAccountHolderRequests20()
        {
            _atm.RequestMoney(_card, 20);
        }

        void ThenTheAtmShouldNotDispenseAnyMoney()
        {
            Assert.AreEqual(_atm.DispenseValue, 0);
        }

        void AndTheAtmShouldSayThereAreInsufficientFunds()
        {
            Assert.AreEqual(_atm.Message, DisplayMessage.InsufficientFunds);
        }

		[AndThen(StepText = "And the account balance should be $20")]
        void AndTheAccountBalanceShouldBe20()
        {
            Assert.AreEqual(_card.AccountBalance, 10);
        }

        void AndTheCardShouldBeReturned()
        {
            Assert.IsFalse(_atm.CardIsRetained);
        }
    }
}