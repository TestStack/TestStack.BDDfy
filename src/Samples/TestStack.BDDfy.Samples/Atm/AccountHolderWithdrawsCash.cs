using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Samples.Atm
{
    [Story(
        AsA = "As an Account Holder",
        IWant = "I want to withdraw cash from an ATM",
        SoThat = "So that I can get money when the bank is closed",
        ImageUri = "https://upload.wikimedia.org/wikipedia/commons/d/d3/49024-SOS-ATM.JPG",
        StoryUri = "http://google.com")]
    public class AccountHolderWithdrawsCash
    {
        private const string GivenTheAccountBalanceIsTitleTemplate = "Given the account balance is ${0}";
        private const string AndTheMachineContainsEnoughMoneyTitleTemplate = "And the machine contains enough money";
        private const string WhenTheAccountHolderRequestsTitleTemplate = "When the account holder requests ${0}";
        private const string AndTheCardShouldBeReturnedTitleTemplate = "And the card should be returned";

        private Card _card;
        private Atm _atm;

        internal void Given_the_Account_Balance_is(int balance)
        {
            _card = new Card(true, balance);
        }

        internal void Given_the_Card_is_disabled()
        {
            _card = new Card(false, 100);
            _atm = new Atm(100);
        }

        internal void And_the_Card_is_valid()
        {
        }

        internal void And_the_machine_contains(int atmBalance)
        {
            _atm = new Atm(atmBalance);
        }

        internal void When_the_Account_Holder_requests(int moneyRequest)
        {
            _atm.RequestMoney(_card, moneyRequest);
        }

        internal void The_ATM_should_dispense(int dispensedMoney)
        {
            _atm.DispenseValue.ShouldBe(dispensedMoney);
        }

        internal void And_the_Account_Balance_should_be(int balance)
        {
            _card.AccountBalance.ShouldBe(balance);
        }

        internal void Then_Card_is_retained(bool cardIsRetained)
        {
            _atm.CardIsRetained.ShouldBe(cardIsRetained);
        }

        internal void And_the_ATM_should_say_the_Card_has_been_retained()
        {
            _atm.Message.ShouldBe(DisplayMessage.CardIsRetained);
        }

        [Fact]
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

        [Fact]
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