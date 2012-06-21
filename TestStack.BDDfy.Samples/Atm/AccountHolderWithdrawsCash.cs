// Copyright (C) 2011, Mehdi Khalili
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of the <organization> nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using NUnit.Framework;
using TestStack.BDDfy.Core;
using TestStack.BDDfy.Scanners.StepScanners.Fluent;

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
        public void AccountHasInsufficientFund()
        {
            new AccountHasInsufficientFund().BDDfy<AccountHolderWithdrawsCash>();
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