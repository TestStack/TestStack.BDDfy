using Bddify.Core;
using NUnit.Framework;

namespace Bddify.Samples.Atm
{
    [Story(
        AsA = "As an Account Holder",
        IWant = "I want to withdraw cash from an ATM",
        SoThat = "So that I can get money when the bank is closed")]
    [TestFixture]
    public class AccountHolderWithdrawsCash
    {
        private const string GivenTheAccountBalanceIsTitleTemplate = "Given the account balance is ${0}";
        private const string AndTheMachineContainsEnoughMoneyTitleTemplate = "And the machine contains enough money";
        private const string WhenTheAccountHolderRequestsTitleTemplate = "When the account holder requests ${0}";
        private const string AndTheCardShouldBeReturnedTitleTemplate = "And the card should be returned";

        private Card _card;
        private Atm _atm;

        public void GivenTheAccountBalanceIs(int balance)
        {
            _card = new Card(true, balance);
        }

        public void GivenTheCardIsDisabled()
        {
            _card = new Card(false, 100);
            _atm = new Atm(100);
        }

        public void AndTheCardIsValid()
        {
        }

        public void AndTheMachineContains(int atmBalance)
        {
            _atm = new Atm(atmBalance);
        }

        public void WhenTheAccountHolderRequests(int moneyRequest)
        {
            _atm.RequestMoney(_card, moneyRequest);
        }

        public void TheAtmShouldDispense(int dispensedMoney)
        {
            Assert.AreEqual(dispensedMoney, _atm.DispenseValue);
        }

        public void AndTheAccountBalanceShouldBe(int balance)
        {
            Assert.AreEqual(balance, _card.AccountBalance);
        }

        public void CardIsRetained(bool cardIsRetained)
        {
            Assert.AreEqual(cardIsRetained, _atm.CardIsRetained);
        }

        void AndTheAtmShouldSayThereAreInsufficientFunds()
        {
            Assert.AreEqual(DisplayMessage.InsufficientFunds, _atm.Message);
        }

        void AndTheAtmShouldSayTheCardHasBeenRetained()
        {
            Assert.AreEqual(DisplayMessage.CardIsRetained, _atm.Message);
        }

        [Test]
        public void AccountHasInsufficientFund()
        {
            new AccountHasInsufficientFund().Bddify();
        }

        [Test]
        public void AccountHasSufficientFund()
        {
           this.Given(s => s.GivenTheAccountBalanceIs(100), GivenTheAccountBalanceIsTitleTemplate)
                    .And(s => s.AndTheCardIsValid())
                    .And(s => s.AndTheMachineContains(100), AndTheMachineContainsEnoughMoneyTitleTemplate)
                .When(s => s.WhenTheAccountHolderRequests(20), WhenTheAccountHolderRequestsTitleTemplate)
                .Then(s => s.TheAtmShouldDispense(20), "Then the ATM should dispense $20")
                    .And(s => s.AndTheAccountBalanceShouldBe(80), "And the account balance should be $80")
                    .And(s => s.CardIsRetained(false), AndTheCardShouldBeReturnedTitleTemplate)
                .Bddify();
        }

        [Test]
        public void CardHasBeenDisabled()
        {
            this.Given(s => s.GivenTheCardIsDisabled())
                .When(s => s.WhenTheAccountHolderRequests(20))
                .Then(s => s.CardIsRetained(true), "Then the ATM should retain the card")
                    .And(s => s.AndTheAtmShouldSayTheCardHasBeenRetained())
                .Bddify();
        }
    }
}