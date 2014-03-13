using NUnit.Framework;
using TestStack.BDDfy.Core;
using TestStack.BDDfy.Scanners;

namespace TestStack.BDDfy.Samples.Atm
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

        public void Given_the_Account_Balance_is(int balance)
        {
            _card = new Card(true, balance);
        }

        public void Given_the_Card_is_disabled()
        {
            _card = new Card(false, 100);
            _atm = new Atm(100);
        }

        public void And_the_Card_is_valid()
        {
        }

        public void And_the_machine_contains(int atmBalance)
        {
            _atm = new Atm(atmBalance);
        }

        public void When_the_Account_Holder_requests(int moneyRequest)
        {
            _atm.RequestMoney(_card, moneyRequest);
        }

        public void The_ATM_should_dispense(int dispensedMoney)
        {
            Assert.AreEqual(dispensedMoney, _atm.DispenseValue);
        }

        public void And_the_Account_Balance_should_be(int balance)
        {
            Assert.AreEqual(balance, _card.AccountBalance);
        }

        public void Then_Card_is_retained(bool cardIsRetained)
        {
            Assert.AreEqual(cardIsRetained, _atm.CardIsRetained);
        }

        void And_the_ATM_should_say_the_Card_has_been_retained()
        {
            Assert.AreEqual(DisplayMessage.CardIsRetained, _atm.Message);
        }

        [Test]
        public void AccountHasSufficientFund()
        {
           this.Given(s => s.Given_the_Account_Balance_is(100), GivenTheAccountBalanceIsTitleTemplate)
                    .And(s => s.And_the_Card_is_valid())
                    .And(s => s.And_the_machine_contains(100), AndTheMachineContainsEnoughMoneyTitleTemplate)
                .When(s => s.When_the_Account_Holder_requests(20), WhenTheAccountHolderRequestsTitleTemplate)
                .Then(s => s.The_ATM_should_dispense(20), "Then the ATM should dispense $20")
                    .And(s => s.And_the_Account_Balance_should_be(80), "And the account balance should be $80")
                    .And(s => s.Then_Card_is_retained(false), AndTheCardShouldBeReturnedTitleTemplate)
                .BDDfy();
        }

        [Test]
        public void CardHasBeenDisabled()
        {
            this.Given(s => s.Given_the_Card_is_disabled())
                .When(s => s.When_the_Account_Holder_requests(20))
                .Then(s => s.Then_Card_is_retained(true), false) // in here I am telling the fluent API that I do not want it to include the input arguments in the step title
                    .And(s => s.And_the_ATM_should_say_the_Card_has_been_retained())
                .BDDfy();
        }
    }
}