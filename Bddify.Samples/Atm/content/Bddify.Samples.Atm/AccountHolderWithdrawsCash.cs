// This class represents a story.

using Bddify.Core;
using NUnit.Framework;

namespace Bddify.Samples.Atm
{
	// You set a class as a story by decorating it with 'Story' attribute
	// Each story can have zero or more scenarios
	// You indicate story's scenarios using WithScenario attribute
    [Story(
        AsA = "As an Account Holder",
        IWant = "I want to withdraw cash from an ATM",
        SoThat = "So that I can get money when the bank is closed")]
    public class AccountHolderWithdrawsCash
    {
        [Test]
        public void CardHasBeenDisabled()
        {
            typeof(CardHasBeenDisabled).Bddify();
        }

        [Test]
        public void AccountHasInsufficientFund()
        {
            typeof(AccountHasInsufficientFund).Bddify();
        }

        [Test]
        public void AccountHasSufficientFunds()
        {
            typeof(AccountHasSufficientFunds).Bddify();
        }
    }
}
