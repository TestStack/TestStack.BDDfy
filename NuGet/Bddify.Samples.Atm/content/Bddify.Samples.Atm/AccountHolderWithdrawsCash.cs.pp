using Bddify.Core;
using NUnit.Framework;

namespace $rootnamespace$.Bddify.Samples.Atm
{
    [Story(
        AsA = "As an Account Holder",
        IWant = "I want to withdraw cash from an ATM",
        SoThat = "So that I can get money when the bank is closed")]
    [WithScenario(typeof(AccountHasSufficientFunds))]
    [WithScenario(typeof(AccountHasInsufficientFund))]
    [WithScenario(typeof(CardHasBeenDisabled))]
    public class AccountHolderWithdrawsCash
    {
        [Test]
        public void Execute()
        {
            this.Bddify();
        }
    }
}
