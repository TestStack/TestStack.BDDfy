using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.ReflectiveScanner
{
    public class WhenStepsReturnTheirText
    {
        class ScenarioWithEnumerableSteps(int input1, string input2)
        {
            private readonly int _input1 = input1;
            private readonly string _input2 = input2;

            IEnumerable<string> Given()
            {
                yield return string.Format("Given inputs {0} and {1}", _input1, _input2);
            }

            [RunStepWithArgs("input 2", 123)]
            IEnumerable<string> When(string input1, int input2)
            {
                yield return string.Format("When {0} is applied on {1}", input1, input2);
            }

            IEnumerable<string> ThenSomeAssertions()
            {
                yield break;
            }
        }

        class ScenarioWithStringSteps
        {
            string GivenTheSystemIsReady()
            {
                return "Given the system is ready for testing";
            }

            string WhenAnActionOccurs()
            {
                return "When an action occurs";
            }

            string ThenTheOutcomeIsCorrect()
            {
                return "Then the outcome is correct";
            }
        }

        class ScenarioWithThrowingEnumerableStep
        {
            IEnumerable<string> GivenSomething()
            {
                yield return "Given something";
            }

            IEnumerable<string> WhenItThrows()
            {
                throw new InvalidOperationException("enumerable step failed");
            }

            IEnumerable<string> ThenNever()
            {
                yield return "Then never";
            }
        }

        class ScenarioWithEnumerableStepThrowingAfterYield
        {
            IEnumerable<string> GivenSomething()
            {
                yield return "Given something";
            }

            IEnumerable<string> WhenItYieldsThenThrows()
            {
                yield return "When it yields then throws";
                throw new InvalidOperationException("failed after yield");
            }

            IEnumerable<string> ThenNever()
            {
                yield return "Then never";
            }
        }

        class ScenarioWithThrowingStringStep
        {
            string GivenSomething() => "Given something";

            string WhenItThrows() => throw new InvalidOperationException("string step failed");

            string ThenNever() => "Then never";
        }

        [Fact]
        public void EnumerableStepsUseDynamicTitles()
        {
            var testObject = new ScenarioWithEnumerableSteps(1, "some input");
            var steps = new DefaultMethodNameStepScanner().Scan(TestContext.GetContext(testObject)).ToList();

            AssertStep(steps[0], "Given inputs 1 and some input", ExecutionOrder.SetupState);
            AssertStep(steps[1], "When input 2 is applied on 123", ExecutionOrder.Transition);
            AssertStep(steps[2], "Then some assertions", ExecutionOrder.Assertion, true);
        }

        [Fact]
        public void StringStepsUseDynamicTitles()
        {
            var testObject = new ScenarioWithStringSteps();
            var steps = new DefaultMethodNameStepScanner().Scan(TestContext.GetContext(testObject)).ToList();

            AssertStep(steps[0], "Given the system is ready for testing", ExecutionOrder.SetupState);
            AssertStep(steps[1], "When an action occurs", ExecutionOrder.Transition);
            AssertStep(steps[2], "Then the outcome is correct", ExecutionOrder.Assertion, true);
        }

        [Fact]
        public void EnumerableStepThrowingAtScanTimeThrowsStepTitleException()
        {
            var testObject = new ScenarioWithThrowingEnumerableStep();
            Should.Throw<StepTitleException>(() =>
                new DefaultMethodNameStepScanner().Scan(TestContext.GetContext(testObject)).ToList());
        }

        [Fact]
        public void EnumerableStepThrowingAfterYieldUsesYieldedTitle()
        {
            // The title is extracted at scan time via FirstOrDefault() which succeeds.
            // The exception occurs at execution time when the step body runs after the yield.
            var testObject = new ScenarioWithEnumerableStepThrowingAfterYield();
            var steps = new DefaultMethodNameStepScanner().Scan(TestContext.GetContext(testObject)).ToList();

            AssertStep(steps[1], "When it yields then throws", ExecutionOrder.Transition);
        }

        [Fact]
        public void StringStepThrowingAtScanTimeThrowsStepTitleException()
        {
            var testObject = new ScenarioWithThrowingStringStep();
            Should.Throw<StepTitleException>(() =>
                new DefaultMethodNameStepScanner().Scan(TestContext.GetContext(testObject)).ToList());
        }

        private static void AssertStep(Step step, string stepTitle, ExecutionOrder order, bool asserts = false, bool shouldReport = true)
        {
            step.Title.ShouldBe(stepTitle);
            step.Asserts.ShouldBe(asserts);
            step.ExecutionOrder.ShouldBe(order);
            step.ShouldReport.ShouldBe(shouldReport);
        }
    }
}