using Shouldly;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Reporters;
using TestStack.BDDfy.Tests.Concurrency;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.ReflectiveScanner
{
    [Collection(TestCollectionName.ModifiesConfigurator)]
    public class AddGherkinPrefixToCustomStepTitleTests
    {
        private class ScenarioWithCustomTitles
        {
            public void GivenTheUserIsLoggedIn() { }

            [Given("the cart has items")]
            public void PopulateCart() { }

            [StepTitle("the payment gateway is available")]
            [Given]
            public void CheckGateway() { }

            [StepTitle("the user completes checkout")]
            [When("the user checks out")]
            public void WhenTheUserClicksCheckout() { }

            public void ThenTheOrderIsConfirmed() { }

            [Then("a confirmation email is sent")]
            public void SendEmail() { }
        }

        private class ScenarioWithPrefixAlreadyInTitle
        {
            [Given("Given the account is active")]
            public void SetupAccount() { }

            [Given("Given the balance is 100")]
            public void SetupBalance() { }

            [When("When the user withdraws 50")]
            public void PerformWithdrawal() { }

            [Then("Then the balance is 50")]
            public void CheckBalance() { }

            [Then("Then a receipt is printed")]
            public void CheckReceipt() { }
        }

        [Fact]
        public void WithPrefixEnabled()
        {
            try
            {
                Configurator.StepTitleFactory.AddGherkinPrefixToSecondarySteps = true;

                var scenario = new ScenarioWithCustomTitles();
                var story = scenario.BDDfy();

                var reporter = new TextReporter();
                reporter.Process(story);
                reporter.ToString().ShouldMatchApproved();
            }
            finally
            {
                Configurator.StepTitleFactory.AddGherkinPrefixToSecondarySteps = true;
            }
        }

        [Fact]
        public void WithPrefixDisabled()
        {
            try
            {
                Configurator.StepTitleFactory.AddGherkinPrefixToSecondarySteps = false;

                var scenario = new ScenarioWithCustomTitles();
                var story = scenario.BDDfy();

                var reporter = new TextReporter();
                reporter.Process(story);
                reporter.ToString().ShouldMatchApproved();
            }
            finally
            {
                Configurator.StepTitleFactory.AddGherkinPrefixToSecondarySteps = true;
            }
        }

        [Fact]
        public void DoesNotDoublePrefixWhenTitleAlreadyStartsWithKeyword()
        {
            var scenario = new ScenarioWithPrefixAlreadyInTitle();
            var story = scenario.BDDfy();

            var reporter = new TextReporter();
            reporter.Process(story);
            reporter.ToString().ShouldMatchApproved();
        }
    }
}
