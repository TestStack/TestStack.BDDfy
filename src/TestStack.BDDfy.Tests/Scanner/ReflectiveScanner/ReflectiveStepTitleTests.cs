using Shouldly;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Reporters;
using TestStack.BDDfy.Tests.Concurrency;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.ReflectiveScanner
{
    [Collection(TestCollectionName.ModifiesConfigurator)]
    public class ReflectiveStepTitleTests
    {
        private class ScenarioWithVariousStepTitleVariations
        {
            public void GivenTheUserIsLoggedIn() { }

            [Given(StepTitle = "the shopping cart contains 3 items")]
            public void PopulateShoppingCart() { }

            [Given("the discount code {0} is applied")]
            [RunStepWithArgs("SAVE20")]
            public void ApplyDiscountCode(string code) { }

            [StepTitle("the payment gateway is available")]
            [Given]
            public void CheckPaymentGateway() { }

            [StepTitle("the shipping address is set to {0}")]
            [Given]
            [RunStepWithArgs("London")]
            public void ConfigureShippingAddress(string city) { }

            [StepTitle("the order total is calculated", false)]
            [Given]
            [RunStepWithArgs(99.99)]
            public void CalculateOrderTotal(double amount) { }

            [StepTitle("the delivery fee is", true)]
            [Given]
            [RunStepWithArgs(4.99)]
            public void CalculateDeliveryFee(double fee) { }

            [StepTitle("the user completes checkout")]
            [When("the user checks out")]
            public void WhenTheUserClicksCheckout() { }

            public void AndWhenThePaymentIsProcessed() { }

            public void ThenTheOrderIsConfirmed() { }

            public void AndThenAConfirmationEmailIsSent() { }

            [Then]
            [RunStepWithArgs(100, 200)]
            [RunStepWithArgs(300, 400)]
            public void VerifyOrderTotals(int subtotal, int total) { }

            [Then]
            [RunStepWithArgs(1, 2, StepTextTemplate = "the inventory is reduced by {0} of {1} items")]
            public void UpdateInventory(int reduced, int total) { }
        }

        [Fact]
        public void AllVariations()
        {
            var scenario = new ScenarioWithVariousStepTitleVariations();
            var story = scenario.BDDfy();

            var reporter = new TextReporter();
            reporter.Process(story);
            reporter.ToString().ShouldMatchApproved();
        }

        [Fact]
        public void AllVariationsWithIncludeInputsDisabled()
        {
            try
            {
                Configurator.StepTitleFactory.IncludeInputsInStepTitle = false;

                var scenario = new ScenarioWithVariousStepTitleVariations();
                var story = scenario.BDDfy();

                var reporter = new TextReporter();
                reporter.Process(story);
                reporter.ToString().ShouldMatchApproved();
            }
            finally
            {
                Configurator.StepTitleFactory.IncludeInputsInStepTitle = true;
            }
        }

        private class ScenarioWithAllVariationsCombined
        {
            public void GivenTheSystemIsInMaintenanceMode() { }

            [StepTitle("the database connection pool is initialized")]
            [Given]
            public void InitializeConnectionPool() { }

            [Given("the cache has {0} entries preloaded")]
            [RunStepWithArgs(50)]
            public void PreloadCache(int count) { }

            [Given(StepTitle = "the retry policy allows {0} attempts")]
            [RunStepWithArgs(3)]
            public void ConfigureRetryPolicy(int maxRetries) { }

            [StepTitle("the request is sent to {0}", true)]
            [When]
            [RunStepWithArgs("/api/orders")]
            public void SendRequest(string endpoint) { }

            [StepTitle("the timeout is set to {0}ms", false)]
            [When]
            [RunStepWithArgs(5000)]
            public void ConfigureTimeout(int milliseconds) { }

            public void WhenTheHealthCheckEndpointIsCalled() { }

            public void ThenTheServiceReturnsA503Status() { }

            [StepTitle("the response includes a retry-after header")]
            [Then]
            public void ValidateRetryAfterHeader() { }

            [Then]
            [RunStepWithArgs("cache-control", "no-store")]
            public void VerifyResponseHeaders(string header, string value) { }

            [Then]
            [RunStepWithArgs("error", "Service Unavailable", StepTextTemplate = "the response body contains {0}: {1}")]
            [RunStepWithArgs("retryAfter", "300", StepTextTemplate = "the response body contains {0}: {1}")]
            public void VerifyResponseBody(string key, string value) { }
        }

        [Fact]
        public void AllVariationsCombined()
        {
            var scenario = new ScenarioWithAllVariationsCombined();
            var story = scenario.BDDfy();

            var reporter = new TextReporter();
            reporter.Process(story);
            reporter.ToString().ShouldMatchApproved();
        }

        [Fact]
        public void AllVariationsCombinedWithIncludeInputsDisabled()
        {
            try
            {
                Configurator.StepTitleFactory.IncludeInputsInStepTitle = false;

                var scenario = new ScenarioWithAllVariationsCombined();
                var story = scenario.BDDfy();

                var reporter = new TextReporter();
                reporter.Process(story);
                reporter.ToString().ShouldMatchApproved();
            }
            finally
            {
                Configurator.StepTitleFactory.IncludeInputsInStepTitle = true;
            }
        }
    }
}
