using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.FluentScanner
{
    public class InlineAssertions
    {
        private int _x, _y, _z;

        [Fact]
        public void CanUseInlineAssertions()
        {
            this.Given(() => { _x = 0; _y = 2; }, "Given x equals 0")
                .When(() => { _z = _x*_y; }, "When x and y are multiplied")
                .Then(() => _z.ShouldBe(0), "Then the result is 0")
                .BDDfy();
        }

        [Fact]
        public void CanUseTitleOnlySteps()
        {
            this.Given("Given x equals 0")
                    .And("and y equals 0")
                .When("When x and y are multiplied")
                    .And("and set to z")
                .Then("Then z equals 0")
                    .And("and we're all cool")
                .BDDfy();
        }

        [Fact]
        public void CanMixThemAllIn()
        {
            this.Given(() => { _x = 0; _y = 2; }, "Given x equals 0")
                    .And("and y equals 0")
                .When(_ => WhenXAndYAreMultiplied())
                    .And("and set to z")
                .Then(() => _z.ShouldBe(0), "Then the result is 0")
                    .And("and we're all cool")
                .BDDfy();

        }

        void WhenXAndYAreMultiplied()
        {
            _z = _x*_y;
        }
    }
}
