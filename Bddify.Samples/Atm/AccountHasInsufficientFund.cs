using Bddify.Scanners.StepScanners.ExecutableAttribute.GwtAttributes;
using NUnit.Framework;

namespace Bddify.Samples.Atm
{
    public class AccountHasInsufficientFund
    {
        private Card _card;
        private Atm _atm;

		// You can override step text using executable attributes
		[Given(StepTitle = "Given the account balance is $10")]
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

		[When(StepTitle = "When the account holder requests $20")]
        void WhenTheAccountHolderRequests20()
        {
            _atm.RequestMoney(_card, 20);
        }

        void ThenTheAtmShouldNotDispenseAnyMoney()
        {
            Assert.AreEqual(0, _atm.DispenseValue);
        }

        void AndTheAtmShouldSayThereAreInsufficientFunds()
        {
            Assert.AreEqual(DisplayMessage.InsufficientFunds, _atm.Message);
        }

		[AndThen(StepTitle = "And the account balance should be $20")]
        void AndTheAccountBalanceShouldBe20()
        {
            Assert.AreEqual(10, _card.AccountBalance);
        }

        void AndTheCardShouldBeReturned()
        {
            Assert.IsFalse(_atm.CardIsRetained);
        }
    }
}