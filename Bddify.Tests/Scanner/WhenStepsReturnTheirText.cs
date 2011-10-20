using System.Collections.Generic;
using System.Linq;
using Bddify.Core;
using Bddify.Scanners.StepScanners.MethodName;
using NUnit.Framework;

namespace Bddify.Tests.Scanner
{
    [TestFixture]
    public class WhenStepsReturnTheirText
    {
        class TestClassWithStepsReturningTheirText
        {
            private readonly int _input1;
            private readonly string _input2;

            public TestClassWithStepsReturningTheirText(int input1, string input2)
            {
                _input1 = input1;
                _input2 = input2;
            }

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

        [Test]
        public void Test()
        {
            var testObject = new TestClassWithStepsReturningTheirText(1, "some input");
            var steps = new DefaultMethodNameStepScanner(testObject).Scan(testObject).ToList();

            AssertStep(steps[0], "Given inputs 1 and some input", ExecutionOrder.SetupState);
            AssertStep(steps[1], "When input 2 is applied on 123", ExecutionOrder.Transition);
            AssertStep(steps[2], "Then some assertions", ExecutionOrder.Assertion, true);
        }

        private static void AssertStep(ExecutionStep step, string stepTitle, ExecutionOrder order, bool asserts = false, bool shouldReport = true)
        {
            Assert.That(step.StepTitle.Trim(), Is.EqualTo(stepTitle));
            Assert.That(step.Asserts, Is.EqualTo(asserts));
            Assert.That(step.ExecutionOrder, Is.EqualTo(order));
            Assert.That(step.ShouldReport, Is.EqualTo(shouldReport));
        }

    }
}