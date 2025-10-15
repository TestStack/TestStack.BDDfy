using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.ReflectiveScanner
{
    public class WhenStepsReturnTheirText
    {
        class TestClassWithStepsReturningTheirText(int input1, string input2)
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

        [Fact]
        public void Test()
        {
            var testObject = new TestClassWithStepsReturningTheirText(1, "some input");
            var steps = new DefaultMethodNameStepScanner().Scan(TestContext.GetContext(testObject)).ToList();

            AssertStep(steps[0], "Given inputs 1 and some input", ExecutionOrder.SetupState);
            AssertStep(steps[1], "When input 2 is applied on 123", ExecutionOrder.Transition);
            AssertStep(steps[2], "Then some assertions", ExecutionOrder.Assertion, true);
        }

        private static void AssertStep(Step step, string stepTitle, ExecutionOrder order, bool asserts = false, bool shouldReport = true)
        {
            step.Title.Trim().ShouldBe(stepTitle);
            step.Asserts.ShouldBe(asserts);
            step.ExecutionOrder.ShouldBe(order);
            step.ShouldReport.ShouldBe(shouldReport);
        }

    }
}